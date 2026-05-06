using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2; // Required for Firebase initialization

namespace GoChauffeurWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SendFCM : ControllerBase
    {
        // Constructor to ensure FirebaseAdmin is initialized
        public SendFCM()
        {
            // Check if FirebaseApp is already initialized to avoid multiple initializations
            if (FirebaseApp.DefaultInstance == null)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "FCMJSON", "fcm.json");
                FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromFile(path)
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> SendFCMNotification([FromBody] FCMRequest request)
        {
            try
            {
                var message = new FirebaseAdmin.Messaging.Message()
                {
                    Token = request.Token,
                    Notification = new FirebaseAdmin.Messaging.Notification()
                    {
                        Title = request.SenderName,
                        Body = request.Text
                    },
                    Data = new Dictionary<string, string>()
            {
                { "EventId", "1" }
            }
                };

                string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
                return Ok($"Successfully sent message: {response}");
            }
            catch (FirebaseMessagingException ex)
            {
                if (ex.MessagingErrorCode == MessagingErrorCode.Unregistered)
                {
                    return BadRequest("Token is invalid or unregistered.");
                }
                else if (ex.MessagingErrorCode == MessagingErrorCode.InvalidArgument)
                {
                    return BadRequest("Invalid token provided.");
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, $"Error sending message: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }
        public class FCMRequest
        {
            public string Token { get; set; }
            public string Text { get; set; }
            public string SenderName { get; set; }
        }

    }
}
