using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using TestingCoreIdentity.Data;

namespace TestingCoreIdentity.Controllers
{
    public class SqlTestController : Controller
    {

        private readonly ApplicationDbContext _context;

        public SqlTestController(ApplicationDbContext context)
        {
            _context = context;
        }

        public string Index()
        {

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Almahfal", "info@test.org"));

            message.To.Add(new MailboxAddress("Saif", "saif@filspay.com"));
            message.Subject = "Sending test email";
            message.Body = new TextPart("plain")
            { Text = "Hello from Body" };

            using (var Client = new SmtpClient())

            {
                Client.Connect("smtp.gmail.com", 587, false);
                Client.Authenticate("info@test.org", "test");
                Client.Send(message);

                Client.Disconnect(true);
            }

            return "Email Sent";

            //working Native SQL statement  - SELECT
            //var Posts = _context.Posts.FromSql("SELECT * FROM Posts").ToList();
            //StringBuilder sb = new StringBuilder();

            //foreach (var Post in Posts)
            //{
            //    sb.Append(Post.Subject + "\n");
            //}
            //return sb.ToString();


            //working Native SQL statement - DELETE
            //int result = _context.Database.ExecuteSqlCommand("DELETE FROM categories WHERE Id = 4");

            //if (result > 0)
            //    return "Deleted";

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