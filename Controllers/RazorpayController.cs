using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using GoChauffeurWebApi.Controllers;
using GoChauffeurWebApi.Data;
using GoChauffeurWebApi.Models;
using GoChauffeurWebApi.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using static RazorpayController;

[Route("api/[controller]")]
[ApiController]
public class RazorpayController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly DataContext _context;
    private readonly HttpClient _httpClient;
    public RazorpayController(IHttpClientFactory httpClientFactory, DataContext context, HttpClient httpClient)
    {
        _httpClientFactory = httpClientFactory;
        _context = context;
        _httpClient = httpClient;
    }




    public class QrCodeImageResponse
    {
        [Newtonsoft.Json.JsonProperty("image_url")]
        public string ImageUrl { get; set; }
    }
    private async Task<dynamic> customerRequest()
    {
        var client = _httpClientFactory.CreateClient();
        var request = new HttpRequestMessage(HttpMethod.Get, "https://api.razorpay.com/v1/customers");
        request.Headers.Add("Authorization", "Basic cnpwX2xpdmVfRGFqTThCcmozV0g0MlE6cnN0bVIxanBXRXRGdU1xR3ZmbTloUFhB");

        var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var responseBody = await response.Content.ReadAsStringAsync();
        dynamic result = Newtonsoft.Json.JsonConvert.DeserializeObject(responseBody);

        return result.items; // Return only the 'items' array
    }

    private async Task<dynamic> Createcustomer(string? name, string? email, string? contact)
    {
        using (var client = new HttpClient())
        {
            var createRequest = new HttpRequestMessage(HttpMethod.Post, "https://api.razorpay.com/v1/customers");
            createRequest.Headers.Add("Authorization", "Basic cnpwX2xpdmVfRGFqTThCcmozV0g0MlE6cnN0bVIxanBXRXRGdU1xR3ZmbTloUFhB");

            var newCustomerData = new
            {
                name = name,
                email = email,
                contact = contact
            };

            string jsonContent = Newtonsoft.Json.JsonConvert.SerializeObject(newCustomerData);
            var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
            createRequest.Content = content;

            var createResponse = await client.SendAsync(createRequest);
            createResponse.EnsureSuccessStatusCode();

            var createResponseBody = await createResponse.Content.ReadAsStringAsync();
            dynamic newCustomer = Newtonsoft.Json.JsonConvert.DeserializeObject(createResponseBody);

            // Return the newly created customer details
            return newCustomer.id;
        }
    }


    [HttpPost("create-qr-code")]
    public async Task<IActionResult> CreateQrCode(int userId, int amount)
    {

        var userdata = _context.Users.Find(userId);
        if(userdata!=null)
        {
            var customers = await customerRequest();
            var customerList = customers as IEnumerable<dynamic>;

            // Filter the customer by phone number
            var customerData = customerList.Where(c => c.contact == userdata.PhoneNumber).ToList();

            if (customerData != null && customerData.Count > 0)
            {
                var customer = customerData[0]; // Select the first matching customer

                // Use an HttpClient from the factory to create a QR code request
                var client = _httpClientFactory.CreateClient();
                var request = new HttpRequestMessage(HttpMethod.Post, "https://api.razorpay.com/v1/payments/qr_codes");

                // Use environment variables or configuration for secure API keys
                string apiKey = Environment.GetEnvironmentVariable("RAZORPAY_API_KEY") ?? "cnpwX2xpdmVfRGFqTThCcmozV0g0MlE6cnN0bVIxanBXRXRGdU1xR3ZmbTloUFhB";
                request.Headers.Add("Authorization", $"Basic {apiKey}");

                dynamic jsonData = new
                {
                    type = "upi_qr",
                    usage = "single_use",
                    fixed_amount = true,
                    payment_amount = amount * 100,
                    customer_id = customer.id, // Use customer ID from the filtered customer
                    close_by = 1726665600, // Set a closing time (timestamp)
                    notes = new
                    {
                        purpose = "Test UPI QR Code notes"
                    }
                };

                string json = Newtonsoft.Json.JsonConvert.SerializeObject(jsonData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                request.Content = content;

                // Send the request and handle the response
                var response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    dynamic result = Newtonsoft.Json.JsonConvert.DeserializeObject(responseBody);
                    string imageUrl = result.image_url;
                    string qrid = result.id;

                    // Return the generated QR code data
                    return Ok(new { url = imageUrl, qrid = qrid });
                }
                else
                {
                    var customerId = await Createcustomer(userdata.Name, userdata.Email, userdata.PhoneNumber);
                    Console.WriteLine("Customer ID: " + customerId);

                    var client1 = _httpClientFactory.CreateClient();
                    var request1 = new HttpRequestMessage(HttpMethod.Post, "https://api.razorpay.com/v1/payments/qr_codes");
                    request.Headers.Add("Authorization", "Basic cnpwX2xpdmVfRGFqTThCcmozV0g0MlE6cnN0bVIxanBXRXRGdU1xR3ZmbTloUFhB");

                    dynamic jsonData1 = new
                    {
                        type = "upi_qr",
                        usage = "single_use",
                        fixed_amount = true,
                        payment_amount = amount * 100,
                        customer_id = customerId, // Use the ID of the first customer
                        close_by = 1726665600,
                        notes = new
                        {
                            purpose = "Test UPI QR Code notes"
                        }
                    };

                    string json1 = Newtonsoft.Json.JsonConvert.SerializeObject(jsonData1);
                    var content1 = new StringContent(json, Encoding.UTF8, "application/json");

                    request1.Content = content1;

                    var response1 = await client.SendAsync(request1);
                    response1.EnsureSuccessStatusCode();

                    string responseBody = await response1.Content.ReadAsStringAsync();
                    dynamic result = Newtonsoft.Json.JsonConvert.DeserializeObject(responseBody);
                    string imageUrl = result.image_url;
                    string qrid = result.id;
                    return Ok(new { url = imageUrl, qrid = qrid });
                }
            }
            else
            {
                var customerId = await Createcustomer(userdata.Name, userdata.Email, userdata.PhoneNumber);
                Console.WriteLine("Customer ID: " + customerId);

                var client = _httpClientFactory.CreateClient();
                var request = new HttpRequestMessage(HttpMethod.Post, "https://api.razorpay.com/v1/payments/qr_codes");
                request.Headers.Add("Authorization", "Basic cnpwX2xpdmVfRGFqTThCcmozV0g0MlE6cnN0bVIxanBXRXRGdU1xR3ZmbTloUFhB");

                dynamic jsonData = new
                {
                    type = "upi_qr",
                    usage = "single_use",
                    fixed_amount = true,
                    payment_amount = amount * 100,
                    customer_id = customerId, // Use the ID of the first customer
                    close_by = 1726665600,
                    notes = new
                    {
                        purpose = "Test UPI QR Code notes"
                    }
                };

                string json = Newtonsoft.Json.JsonConvert.SerializeObject(jsonData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                request.Content = content;

                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                dynamic result = Newtonsoft.Json.JsonConvert.DeserializeObject(responseBody);
                string imageUrl = result.image_url;
                string qrid = result.id;
                return Ok(new { url = imageUrl,qrid = qrid });
            }
        }
        else
        {
            return BadRequest("User Not Registered");
        }
    }

    [HttpPost("Close-Payment/complete-trip")]
    public async Task<IActionResult> Completepayment(int userId, int DriverId, string qrid)
    {
        try
        {
            var result = await GetQrCodePayments(qrid);
            var id = result["items"]?.FirstOrDefault()?["id"]?.ToString(); // Adjust based on actual structure
            var amount= result["items"]?.FirstOrDefault()?["amount"]?.ToString();
            if (id == null)
            {
                return NotFound("Payment not found.");
            }
            var closeqr = await CloseQrCode(qrid);
            // Process the payment with id and other parameters as needed
            
         
            
            return Ok("done");
        }
        catch (Exception ex) 
        {
            return BadRequest(ex.Message);
        }
       
    }

    private async Task<IActionResult> CloseQrCode(string qrCodeId)
    {
        // Construct the request URL
        var requestUrl = $"https://api.razorpay.com/v1/payments/qr_codes/{qrCodeId}/close";

        // Prepare the HTTP request
        var request = new HttpRequestMessage(HttpMethod.Post, requestUrl);
        request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "cnpwX2xpdmVfRGFqTThCcmozV0g0MlE6cnN0bVIxanBXRXRGdU1xR3ZmbTloUFhB");

        // Send the request
        var response = await _httpClient.SendAsync(request);

        // Ensure success status code
        response.EnsureSuccessStatusCode();

        // Read and return the response content
        var responseContent = await response.Content.ReadAsStringAsync();
        return Ok(responseContent);
    }

    private async Task<JObject> GetQrCodePayments(string qrCodeId)
    {
        // Construct the request URL
        var requestUrl = $"https://api.razorpay.com/v1/payments/qr_codes/{qrCodeId}/payments?count=1";

        // Prepare the HTTP request
        var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
        request.Headers.Authorization = new AuthenticationHeaderValue("Basic", "cnpwX2xpdmVfRGFqTThCcmozV0g0MlE6cnN0bVIxanBXRXRGdU1xR3ZmbTloUFhB");

        // Send the request
        var response = await _httpClient.SendAsync(request);

        // Ensure success status code
        response.EnsureSuccessStatusCode();

        // Read and return the response content
        var responseContent = await response.Content.ReadAsStringAsync();
        return JObject.Parse(responseContent); // Return the parsed JObject
    }

}
