using GoChauffeurWebApi.Data;
using GoChauffeurWebApi.Models;
using GoChauffeurWebApi.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Nest;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Reflection;

namespace GunturPickles_Grocery_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Login : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;

        public Login(DataContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        [HttpPost("{mobileNumber}")]
        public async Task<dynamic> LoginUser(string mobileNumber, string? refercode, int? flag)
        {
            try
            {

                var loginData = _context.Users.Where(y => y.PhoneNumber == mobileNumber).FirstOrDefault();

                if (loginData != null)
                {

                    int _min = 1000;
                    int _max = 9999;
                    Random _rdm = new Random();
                    Int32 OTP = _rdm.Next(_min, _max);
                    string text = "Dear Patron, Please enter the OTP: " + OTP + "to log into your Gochauffeurs account. Kindly do not share with anyone. We are here to assist you! Team Gochauffeurs!";
                    byte[] utf8Bytes = System.Text.Encoding.UTF8.GetBytes(text);
                    var s_unicode2 = System.Text.Encoding.UTF8.GetString(utf8Bytes);
                    var url = "http://text.justsms.co.in/api.php?username=varaahi&apikey=cb2968ba7b0cbc38adb4&senderid=GOCHFF&templateid=1707171475616206642&mobile=" + mobileNumber + "&message= " + s_unicode2;
                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri(url);
                    HttpResponseMessage response = client.GetAsync(url).Result;
                    var userdata = loginData;
                    var token = GenerateAccessToken(userdata); // Generate token
                    return Ok(new { userdata, OTP, token }); // Return token along with OTP and user data

                }
                else
                {


                    var userdata = new User();

                    userdata.PhoneNumber = mobileNumber;
                    userdata.ReferCode = GenerateUniqueReferCode();
                    _context.Users.Add(userdata);
                    // Use SaveChanges instead of SaveChangesAsync as this is not awaited
                    if (_context.SaveChanges() > 0) ;

                    if (refercode != null && refercode != String.Empty)
                    {
                        var referdata = _context.Users.Where(c => c.ReferCode == refercode).FirstOrDefault();
                        if (referdata != null)
                        {
                            var customerwalletdata = _context.CustomerWallets.Where(c => c.UserId == referdata.Id).FirstOrDefault();
                            if (customerwalletdata != null)
                            {
                                var referearningdata = _context.ReferEarnings.FirstOrDefault();
                                if (referearningdata != null)
                                {
                                    customerwalletdata.WalletBalance = customerwalletdata.WalletBalance + referearningdata.ReferredByEarning;
                                    _context.Entry(customerwalletdata).State = EntityState.Modified;
                                    await _context.SaveChangesAsync();
                                    var customertransactionhistory = new CustomerwalletTransactionHistory();
                                    customertransactionhistory.UserId = referdata.Id;
                                    customertransactionhistory.WalletAmount = referearningdata.ReferredByEarning;
                                    customertransactionhistory.Description = "Refer Earnings";
                                    _context.customerwalletTransactionHistories.Add(customertransactionhistory);
                                    await _context.SaveChangesAsync();
                                    var logincustomerwalletdata = _context.CustomerWallets.Where(c => c.UserId == userdata.Id).FirstOrDefault();
                                    if (logincustomerwalletdata != null)
                                    {
                                        logincustomerwalletdata.WalletBalance = logincustomerwalletdata.WalletBalance + referearningdata.ReferredEarning;
                                        _context.Entry(logincustomerwalletdata).State = EntityState.Modified;
                                        await _context.SaveChangesAsync();
                                        customertransactionhistory.UserId = userdata.Id;
                                        customertransactionhistory.WalletAmount = referearningdata.ReferredByEarning;
                                        customertransactionhistory.Description = "Refer Earnings";
                                        _context.customerwalletTransactionHistories.Add(customertransactionhistory);
                                        await _context.SaveChangesAsync();
                                    }
                                    else
                                    {
                                        var logincustomerwallet = new CustomerWallet();
                                        logincustomerwallet.UserId = userdata.Id;
                                        logincustomerwallet.WalletBalance = referearningdata.ReferredEarning;
                                        _context.CustomerWallets.Add(logincustomerwallet);
                                        await _context.SaveChangesAsync();
                                        customertransactionhistory.UserId = userdata.Id;
                                        customertransactionhistory.WalletAmount = referearningdata.ReferredByEarning;
                                        customertransactionhistory.Description = "Refer Earnings";
                                        _context.customerwalletTransactionHistories.Add(customertransactionhistory);
                                        await _context.SaveChangesAsync();
                                    }
                                }

                            }
                            else
                            {
                                var referearningdata = _context.ReferEarnings.FirstOrDefault();
                                if (referearningdata != null)
                                {
                                    var custmerwalletdata = _context.CustomerWallets.Where(c => c.UserId == referdata.Id).FirstOrDefault();
                                    if (custmerwalletdata != null)
                                    {
                                        custmerwalletdata.WalletBalance = custmerwalletdata.WalletBalance + referearningdata.ReferredEarning;
                                        _context.Entry(custmerwalletdata).State = EntityState.Modified;
                                        await _context.SaveChangesAsync();
                                        var customertransactionhistory = new CustomerwalletTransactionHistory();
                                        customertransactionhistory.UserId = referdata.Id;
                                        customertransactionhistory.WalletAmount = referearningdata.ReferredByEarning;
                                        customertransactionhistory.Description = "Refer Earnings";
                                        _context.customerwalletTransactionHistories.Add(customertransactionhistory);
                                        await _context.SaveChangesAsync();
                                    }
                                    else
                                    {

                                        var cwallet = new CustomerWallet();
                                        cwallet.UserId = referdata.Id;
                                        cwallet.WalletBalance = referearningdata.ReferredEarning;
                                        _context.CustomerWallets.Add(cwallet);
                                        await _context.SaveChangesAsync();
                                        var customertransactionhistory = new CustomerwalletTransactionHistory();
                                        customertransactionhistory.UserId = referdata.Id;
                                        customertransactionhistory.WalletAmount = referearningdata.ReferredByEarning;
                                        customertransactionhistory.Description = "Refer Earnings";
                                        _context.customerwalletTransactionHistories.Add(customertransactionhistory);
                                        await _context.SaveChangesAsync();
                                    }
                                    var logincustomerwalletdata = _context.CustomerWallets.Where(c => c.UserId == userdata.Id).FirstOrDefault();
                                    if (logincustomerwalletdata != null)
                                    {
                                        logincustomerwalletdata.WalletBalance = logincustomerwalletdata.WalletBalance + referearningdata.ReferredEarning;
                                        _context.Entry(logincustomerwalletdata).State = EntityState.Modified;
                                        await _context.SaveChangesAsync();
                                        var customertransactionhistory = new CustomerwalletTransactionHistory();
                                        customertransactionhistory.UserId = userdata.Id;
                                        customertransactionhistory.WalletAmount = referearningdata.ReferredByEarning;
                                        customertransactionhistory.Description = "Refer Earnings";
                                        _context.customerwalletTransactionHistories.Add(customertransactionhistory);
                                        await _context.SaveChangesAsync();
                                    }
                                    else
                                    {
                                        var logincustomerwallet = new CustomerWallet();
                                        logincustomerwallet.UserId = userdata.Id;
                                        logincustomerwallet.WalletBalance = referearningdata.ReferredEarning;
                                        _context.CustomerWallets.Add(logincustomerwallet);
                                        await _context.SaveChangesAsync();
                                        var customertransactionhistory = new CustomerwalletTransactionHistory();
                                        customertransactionhistory.UserId = userdata.Id;
                                        customertransactionhistory.WalletAmount = referearningdata.ReferredByEarning;
                                        customertransactionhistory.Description = "Refer Earnings";
                                        _context.customerwalletTransactionHistories.Add(customertransactionhistory);
                                        await _context.SaveChangesAsync();
                                    }
                                }
                            }

                        }

                    }


                    int _min = 1000;
                    int _max = 9999;
                    Random _rdm = new Random();
                    Int32 OTP = _rdm.Next(_min, _max);
                    string text = "Dear Patron, Please enter the OTP: " + OTP + "to log into your Gochauffeurs account. Kindly do not share with anyone. We are here to assist you! Team Gochauffeurs!";
                    byte[] utf8Bytes = System.Text.Encoding.UTF8.GetBytes(text);
                    var s_unicode2 = System.Text.Encoding.UTF8.GetString(utf8Bytes);
                    var url = "http://text.justsms.co.in/api.php?username=varaahi&apikey=cb2968ba7b0cbc38adb4&senderid=GOCHFF&templateid=1707171475616206642&mobile=" + mobileNumber + "&message= " + s_unicode2;
                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri(url);
                    HttpResponseMessage response = client.GetAsync(url).Result;
                    var token = GenerateAccessToken(userdata); // Generate token
                    return Ok(new { userdata, OTP, token }); // Return token along with OTP and user data
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private string GenerateUniqueReferCode()
        {
            const int length = 8;
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();

            return new string(Enumerable.Repeat(chars, length)
                                        .Select(s => s[random.Next(s.Length)])
                                        .ToArray());
        }

        [HttpPost("{mobileNumber}/DriverLogin")]
        public async Task<ActionResult<Driver>> LoginDriver(string mobileNumber,string notificationtoken)
        {
            try
            {

                var loginData = _context.Drivers.Where(y => y.PhoneNumber == mobileNumber).FirstOrDefault();

                if (loginData != null)
                {
                    string randomWord = GenerateRandomWord();

                    // Generate three dummy random numbers
                    string dummyNumbers = GenerateDummyNumbers();
                    if (loginData.ReferCode == null)
                    {
                        loginData.ReferCode = $"{randomWord}{dummyNumbers}_{loginData.DriverId}";
                    }
                        loginData.Token= notificationtoken;
                    _context.Entry(loginData).State = EntityState.Modified;

                   
                        await _context.SaveChangesAsync();
                    int _min = 1000;
                    int _max = 9999;
                    Random _rdm = new Random();
                    Int32 OTP = _rdm.Next(_min, _max);
                    string text = "Dear Patron, Please enter the OTP: " + OTP + " to log into your Gochauffeurs Partner account. Kindly do not share with anyone. We are here to assist you! Gochauffeurs!";
                    byte[] utf8Bytes = System.Text.Encoding.UTF8.GetBytes(text);
                    var s_unicode2 = System.Text.Encoding.UTF8.GetString(utf8Bytes);
                    var url = "http://text.justsms.co.in/api.php?username=varaahi&apikey=cb2968ba7b0cbc38adb4&senderid=GOCHFF&templateid=1707171475606556910&mobile=" + mobileNumber + "&message=" + s_unicode2;
                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri(url);
                    HttpResponseMessage response = client.GetAsync(url).Result;
                    var driverData = loginData;
                    var statusId = 0;
                    if (driverData.VerificationStatusIds != null)
                    {

                        statusId = Convert.ToInt32(driverData.VerificationStatusIds.Split(',').Last());
                    }
                    var token = GenerateAccessTokenDriver(driverData);
                    // Generate token
                    return Ok(new { driverData, OTP, token, statusId }); // Return token along with OTP and user data

                }
                else
                {

                    var driverData = new Driver();
                    driverData.IsVerified = false;
                    driverData.PhoneNumber = mobileNumber;
                    string randomWord = GenerateRandomWord();

                    // Generate three dummy random numbers
                    string dummyNumbers = GenerateDummyNumbers();
                    _context.Drivers.Add(driverData);
                    await _context.SaveChangesAsync();

                    var ddata = _context.Drivers.Where(x => x.PhoneNumber == driverData.PhoneNumber).FirstOrDefault();
                    if (ddata != null)
                    {
                        driverData.ReferCode = $"{randomWord}{dummyNumbers}_{driverData.DriverId}";

                        string uniqueId = "GoC" + DateTime.UtcNow.ToString("yyyyMMdd") + driverData.DriverId;
                        driverData.DriverRecID = uniqueId;
                        _context.Entry(driverData).State = EntityState.Modified;
                        _context.SaveChangesAsync();
                    }

                    var statusId = 0;
                    if (driverData.VerificationStatusIds != null)
                    {

                        statusId = Convert.ToInt32(driverData.VerificationStatusIds.Split(',').Last());
                    }


                    int _min = 1000;
                    int _max = 9999;
                    Random _rdm = new Random();
                    Int32 OTP = _rdm.Next(_min, _max);
                    string text = "Dear Patron, Please enter the OTP: " + OTP + " to log into your Gochauffeurs Partner account. Kindly do not share with anyone. We are here to assist you! Gochauffeurs!";
                    byte[] utf8Bytes = System.Text.Encoding.UTF8.GetBytes(text);
                    var s_unicode2 = System.Text.Encoding.UTF8.GetString(utf8Bytes);
                    var url = "http://text.justsms.co.in/api.php?username=varaahi&apikey=cb2968ba7b0cbc38adb4&senderid=GOCHFF&templateid=1707171475606556910&mobile=" + mobileNumber + "&message=" + s_unicode2;

                    HttpClient client = new HttpClient();

                    client.BaseAddress = new Uri(url);
                    HttpResponseMessage response = client.GetAsync(url).Result;
                    var token = GenerateAccessTokenDriver(driverData); // Generate token
                    return Ok(new { driverData, OTP, token, statusId }); // Return token along with OTP and user data

                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        private string GenerateRandomWord()
        {
            // Add your logic to generate a random word here
            // For simplicity, using a predefined list of words
            List<string> wordList = new List<string> { "Limo", "Chauffeur", "Sedan", "Luxury", "Service", "Travel", "Comfort", "Elegance", "Ride" };
            Random random = new Random();
            int randomIndex = random.Next(wordList.Count);

            return wordList[randomIndex];
        }

        private string GenerateDummyNumbers()
        {
            // Generate three dummy random numbers
            Random random = new Random();
            int randomNumber = random.Next(100, 1000); // Generates a random number between 100 and 999

            // Concatenate the dummy numbers
            return randomNumber.ToString();
        }
        [HttpPost("{emailOrPhoneNumber}/{password}")]
        public async Task<IActionResult> LoginUser(string emailOrPhoneNumber, string password)
        {
            try
            {
                var user = await _context.Admins.FirstOrDefaultAsync(x => (x.Email.ToLower() == emailOrPhoneNumber.ToLower() || x.Mobile == emailOrPhoneNumber) && x.Password == password);

                if (user == null)
                {
                    // User not found or password doesn't match
                    return BadRequest("Invalid email or password");
                }

                // Authentication successful
                var token = GenerateAccessToken1(user);
                return Ok(new { user, token });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        private string GenerateAccessToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        }),
                Expires = DateTime.UtcNow.AddDays(30),
                Audience = _configuration["Jwt:Audience"], // Set the audience value
                Issuer = _configuration["Jwt:Issuer"], // Set the issuer value
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private string GenerateAccessTokenDriver(Driver user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
            new Claim(ClaimTypes.NameIdentifier, user.DriverId.ToString())
        }),
                Expires = DateTime.UtcNow.AddDays(30),
                Audience = _configuration["Jwt:Audience"], // Set the audience value
                Issuer = _configuration["Jwt:Issuer"], // Set the issuer value
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        private string GenerateAccessToken1(Admin user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
            new Claim(ClaimTypes.NameIdentifier, user.AdminId.ToString())
        }),
                Expires = DateTime.UtcNow.AddDays(30),
                Audience = _configuration["Jwt:Audience"], // Set the audience value
                Issuer = _configuration["Jwt:Issuer"], // Set the issuer value
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

    }
}
