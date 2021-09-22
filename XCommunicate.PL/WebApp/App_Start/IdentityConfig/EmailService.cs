using Microsoft.AspNet.Identity;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace WebApp.App_Start.IdentityConfig
{
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            return Task.Factory.StartNew(() =>
            {
                SendMail(message);
            });
        }

        private void SendMail(IdentityMessage message)
        {
            string email = ConfigurationManager.AppSettings["Email"].ToString();
            string password = ConfigurationManager.AppSettings["Password"].ToString();

            MailAddress fromAddress = new MailAddress(email, "x-com");
            MailAddress toAddress = new MailAddress(message.Destination);

            MailMessage mailMessage = new MailMessage(fromAddress, toAddress);
            mailMessage.Body = message.Body;
            mailMessage.IsBodyHtml = true;
            mailMessage.Subject = message.Subject;

            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Host = "smtp.gmail.com";
            smtpClient.Port = 587;
            smtpClient.EnableSsl = true;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.Credentials = new NetworkCredential(fromAddress.Address, password);
            smtpClient.Send(mailMessage);
        }
    }
}