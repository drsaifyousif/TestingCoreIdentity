using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SendGrid;
using SendGrid.Helpers.Mail;
using TestingCoreIdentity.Data;
using TestingCoreIdentity.Models;
using TestingCoreIdentity.Services;

namespace TestingCoreIdentity.Controllers
{
    public class SqlController : Controller
    {
        private readonly ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;

        private RoleManager<IdentityRole> _roleManager;
        private string htmlContent;

     

        public SqlController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            //_roleManager = roleMgr;
            _userManager = userManager;

            //var id =  BackgroundJob.Schedule(() => Sendemail, TimeSpan.FromMinutes(1));

            var options = new BackgroundJobServerOptions
            {
                SchedulePollingInterval = TimeSpan.FromMinutes(1)
            };

            var server = new BackgroundJobServer(options);

            RecurringJob.AddOrUpdate("Saif1", () => Sendemail(), Cron.Minutely);
            
        }

        public string Index()
        {
            BackgroundJob.Schedule(() => Sendemail(), TimeSpan.FromMinutes(1));

            return "ok";
        }


        

        public void Sendemail()
        {
     
            string htmlContent = "Hi";
            var apiKey = "xxxxxxxxxxxxxxxxxxxxx";
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("from@website.com", "Support");
            var to = new EmailAddress("to@website.com");
            var plainTextContent = Regex.Replace(htmlContent, "<[^>]*>", "");
            var msg = MailHelper.CreateSingleEmail(from, to, "Hi", plainTextContent, htmlContent);
            var response = client.SendEmailAsync(msg);


           

            //  return "ok";
            // return "Test";

            //working Native SQL statement  - SELECT
            //var Categories = _context.Categories.FromSql("SELECT * FROM Categories").ToList();
            //StringBuilder sb = new StringBuilder();

            //foreach (var cat in Categories)
            //{
            //    sb.Append(cat.Name + "\n");
            //}
            //return sb.ToString();


            //working Native SQL statement - DELETE
            //int result = _context.Database.ExecuteSqlCommand("DELETE FROM categories WHERE Id = 5");

            //if (result > 0)
            //    return "dELETED";

            //else
            //    return "Not found";



            //working Native SQL statement - Select
            //DbConnection con = _context.Database.GetDbConnection();
            //con.Open();
            //DbCommand com = con.CreateCommand();
            //com.CommandText = "SELECT Name FROM Categories";
            //com.CommandType = System.Data.CommandType.Text;
            //DbDataReader dr = com.ExecuteReader();
            //StringBuilder data = new StringBuilder();

            //while (dr.Read())
            //{
            //    data.AppendLine(dr.GetString(0));
            //}

            //return data.ToString();


        }


    }
}