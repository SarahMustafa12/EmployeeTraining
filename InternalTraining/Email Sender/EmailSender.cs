﻿
using System.Net.Mail;
using System.Net;

namespace E_TicketMovies.Email_Sender
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("sm3380719@gmail.com", "qqdv kayh yzot gupx")
            };

            return client.SendMailAsync(
                new MailMessage(from: "sm3380719@gmail.com",
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

