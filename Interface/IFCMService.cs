namespace GoChauffeurWebApi.Interface
{
    public interface IFCMService
    {
        Task<string> SendNotificationAsync(string token, string senderName, string text);
    }
}
