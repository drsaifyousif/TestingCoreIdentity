using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TestingCoreIdentity.Data;
using TestingCoreIdentity.Models;
using TestingCoreIdentity.Helpers;

namespace TestingCoreIdentity.Controllers
{
    public class UserRolesController : Controller
    {

        private readonly ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;

        //private RoleManager<IdentityRole> _roleManager;


        public UserRolesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        //public IActionResult CreateAsync() => View();

        public async Task<IActionResult> CreateAsync()
        {

            // ViewData["UserId"] = new SelectList(_context.ApplicationUser, "Id", "Id");

            //await Helpers.SendEmail.SendGrid("saif@filspay.com", "Hi from System", "Hi from System Body");
         //   BackgroundJob.Schedule(() => , TimeSpan.FromMinutes(1));

            return View();
        }



        [HttpPost]
        public async Task<IActionResult> CreateAsync(string Email, string RoleName)

        {
            try
            {
                ApplicationUser user = await _userManager.FindByEmailAsync(Email);
                await _userManager.AddToRoleAsync(user, RoleName);

                return RedirectToAction("Index");
            }
            catch (Exception)
            {

                return RedirectToAction("Index");
            }
           

        }
    }
}