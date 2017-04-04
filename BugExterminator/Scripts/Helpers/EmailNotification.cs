using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.Configuration;


//TODO: Write the class that is responsible for sending the composed email
public class NotificationSvc
{
    public async Task SendAsync(MailMessage message)
    {
        var gmailUserName = WebConfigurationManager.AppSettings["username"];
        var gmailPassword = WebConfigurationManager.AppSettings["password"];
        var host = WebConfigurationManager.AppSettings["host"];
        int port = Convert.ToInt32(WebConfigurationManager.AppSettings["port"]);

        using (var smtp = new SmtpClient()
        {
            Host = host,
            Port = port,
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(gmailUserName, gmailPassword)
        })
        {
            try
            {
                await smtp.SendMailAsync(message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                await Task.FromResult(0);
            }
        }
    }
}