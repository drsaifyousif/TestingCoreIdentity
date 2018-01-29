using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TestingCoreIdentity.Services
{
    public class SendGridMailSender
    {
        public void  SendEmail(string To,string Subject,string htmlContent)
        {
           
            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("mohamed.asnd@gmail.com", "Support");
            var to = new EmailAddress(To);
            var plainTextContent = Regex.Replace(htmlContent, "<[^>]*>", "");
            var msg = MailHelper.CreateSingleEmail(from, to, Subject, plainTextContent, htmlContent);
            var response = client.SendEmailAsync(msg);

           
        }

    }
}
