using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;

namespace AppMGL.Manager.Infrastructure.Helper
{
	public static class EmailHelper
	{
        public static void Send(ClaimsPrincipal principal, MailMessage mailMessage)
        {
            using (var client = new SmtpClient())
            {
                if (principal.Identity.IsAuthenticated)
                {
                    string username = principal.Claims.Where(c => c.Type == "UsrSmtpUsername").Single().Value;
                    string password = principal.Claims.Where(c => c.Type == "UsrSmtpPassword").Single().Value;

                    if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                    {
                     string   pass = SecurityHelper.DecryptString(password, "sblw-3hn8-sqoy19");
                        client.Credentials = new NetworkCredential(username, pass);
                        mailMessage.From = new MailAddress(username);
                    }
                }

                client.Send(mailMessage);
            }
        }
	}
}