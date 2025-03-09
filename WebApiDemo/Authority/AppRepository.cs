using Microsoft.AspNetCore.DataProtection;

namespace WebApiDemo.Authority
{
    public static class AppRepository
    {
        private static List<Application> _applications = new List<Application>()
        {
            new Application
            {
            ApplicationId = 1,
            ApplicationName = "MVCWebApp",
            ClientId = "41EE49E7-5325-441E-8340-6026C1EE8F31",
            Secret = "D7053E48-B933-4E34-B90D-EFB26051292A",
            Scopes = "read,write,delete"
            }
        };

        public static Application? GetApplicationByClientId(string clientId)
        {
            return _applications.FirstOrDefault(a => a.ClientId == clientId);
        }
    }
}
