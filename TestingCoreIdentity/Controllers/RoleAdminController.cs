using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using TestingCoreIdentity.Models;


namespace ARID.Controllers
{

    [Authorize]
    public class RoleAdminController : Controller
    {
        private RoleManager<IdentityRole> roleManager;
        private UserManager<ApplicationUser> userManager;

        public RoleAdminController(RoleManager<IdentityRole> roleMgr, UserManager<ApplicationUser> userMrg)
        {
            roleManager = roleMgr; 
            userManager = userMrg;
        }

        public ViewResult Index()
        {
            return View(roleManager.Roles);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create([Required]string name)
        {
            if (ModelState.IsValid)
            {
                IdentityResult result
                = await roleManager.CreateAsync(new IdentityRole(name));
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    AddErrorsFromResult(result);
                }
            }
            return View(name);
        }

        // GET: RoleAdmin/Rename/5
        public async Task<IActionResult> Rename(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var role = await roleManager.Roles.SingleOrDefaultAsync(m => m.Id == id);
            if (role == null)
            {
                return NotFound();
            }
            return View(role);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Rename(string id, [Bind("Id,Name,NormalizedName")] IdentityRole role)
        {
            if (id != role.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    IdentityRole thisRole = await roleManager.FindByIdAsync(role.Id);
                    thisRole.Name = role.Name;
                    await roleManager.UpdateAsync(thisRole);
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
                return RedirectToAction("Index");
            }
            return View(role);
        }


       
      
        // GET: Cities/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var role = await roleManager.Roles
                .SingleOrDefaultAsync(m => m.Id == id);
            if (role == null)
            {
                return NotFound();
            }

            return View(role);
        }

        // POST: Cities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var role = await roleManager.Roles.SingleOrDefaultAsync(m => m.Id == id);
            await roleManager.DeleteAsync(role);
            return RedirectToAction("Index");
        }

        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }

       
        }
    }