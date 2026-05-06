using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace GoChauffeurWebApi.Interface
{
    public class FCMService : IFCMService
    {
        private readonly HttpClient _httpClient;

        public FCMService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> SendNotificationAsync(string token, string senderName, string text)
        {
            var request = new
            {
                Token = token,
                Text = text,
                SenderName = senderName,
            };

            var json = JsonConvert.SerializeObject(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://api.gochauffeurs.in/api/SendFCM", content); // Adjust the URL as needed

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new Exception($"Failed to send notification: {errorMessage}");
            }
        }
    }
}
