
using System.Net.Mail;
using System.Net;

namespace InternalTraining.Email_Sender
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("semamentor@gmail.com", "mlgr lpfy sifl uowm")
            };

            return client.SendMailAsync(
                new MailMessage(from: "semamentor@gmail.com",
                                to: email,
                                subject,
                                message
                                )
                {
                    IsBodyHtml = true
                });
        }
    }
}

