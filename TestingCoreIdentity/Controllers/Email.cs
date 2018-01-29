
using Microsoft.AspNetCore.Mvc;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Text.RegularExpressions;


namespace TestingCoreIdentity.Controllers
{
    public class Email : Controller
    {

        public string Index()

        { 
        string htmlContent = "Hi";
        var apiKey = "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";
        var client = new SendGridClient(apiKey);
        var from = new EmailAddress("mohamed.asnd@gmail.com", "Support");
        var to = new EmailAddress("info@filspay.com");
        var plainTextContent = Regex.Replace(htmlContent, "<[^>]*>", "");
        var msg = MailHelper.CreateSingleEmail(from, to, "Hi", plainTextContent, htmlContent);
        var response = client.SendEmailAsync(msg);

            return "Email Sent";
        }
    }
}
