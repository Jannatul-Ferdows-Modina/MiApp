using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace AppMGL.DAL.Helper
{
    public class EmailService : IIdentityMessageService
    {
        public async Task SendAsync(IdentityMessage message)
        {
            MailMessage mailMessage = new MailMessage();

            mailMessage.To.Add(message.Destination);
            //mailMessage.From = new MailAddress("postmaster@sandboxd08f213f287a410896eb92d7a8d7d9da.mailgun.org", "MGL");
            mailMessage.Subject = message.Subject;
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = message.Body;

            SmtpClient smtpClient = new SmtpClient();
            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}