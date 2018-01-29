using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TestingCoreIdentity.Data;
using TestingCoreIdentity.Helpers;
using TestingCoreIdentity.Models;
using TestingCoreIdentity.Services;

namespace TestingCoreIdentity.Controllers
{
    [Authorize(Roles ="Sales")]
    public class PostsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IHostingEnvironment _environment;
        private readonly UserManager<ApplicationUser> _userManager;
    
        public PostsController(ApplicationDbContext context, IHostingEnvironment environment, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _environment = environment;
            _userManager = userManager;

        }

        // GET: Posts
        public async Task<IActionResult> Index(int? id)
        {

            var applicationDbContext = new object();
            if (id > 0)
            {
                applicationDbContext = _context.Posts
                    .Include(n => n.Category)
                    .Include(n => n.User)
                    .Where(n => n.CategoryId == id)
                    .OrderBy(n => n.PublicationDate);
            }
            else
            {
                applicationDbContext = _context.Posts.Include(p => p.Category).Include(p => p.User);
            }

            return View(await ((IQueryable<Post>)applicationDbContext).ToListAsync());
            //var applicationDbContext = _context.Posts.Include(p => p.Category).Include(p => p.User);
            //return View(await applicationDbContext.ToListAsync());
        }

        // GET: Posts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.Category)
                .Include(p => p.User)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            // Execute Stored procedure in the database (Delete, Update)
            var con = _context.Database.GetDbConnection();
            con.Open();
            var comm = con.CreateCommand();
            comm.CommandText = "IncreaseReaders"; //Stored procedure name
            comm.CommandType = System.Data.CommandType.StoredProcedure;
            comm.Parameters.Add(new SqlParameter("Id", id));
            comm.ExecuteNonQuery();
            // End of SP code

            //_context.Posts.FromSql("IncreaseReaders @Id", new SqlParameter("@Id", id));
            return View(post);
        }

        // GET: Posts/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            ViewData["UserId"] = new SelectList(_context.ApplicationUser, "Id", "Id");
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CategoryId,Subject,Body,AuthorName,ImageUrl")] Post post, IFormFile myfile)
        {
            if (ModelState.IsValid)
            {

                post.ImageUrl = await UserFile.UploadeNewImageAsync(post.ImageUrl,
              myfile, _environment.WebRootPath, Properties.Resources.ImgFolder, 100, 100);

                post.PublicationDate = DateTime.Today.Date;
                post.UserId = _userManager.GetUserId(User);

                _context.Add(post);
                await _context.SaveChangesAsync();

                //      BackgroundJob.Schedule(() => MMM.SendEmail("info@filspay.com", "New Post Added", "Post Content"), TimeSpan.FromMinutes(1));

                  //  BackgroundJob.Schedule(() => SendEmail, TimeSpan.FromMinutes(1));


             


                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", post.CategoryId);
            ViewData["UserId"] = new SelectList(_context.ApplicationUser, "Id", "Id", post.UserId);
            return View(post);
        }



       


            // GET: Posts/Edit/5
            public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts.SingleOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", post.CategoryId);
            ViewData["UserId"] = new SelectList(_context.ApplicationUser, "Id", "Id", post.UserId);
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CategoryId,Subject,Body,AuthorName,ImageUrl")] Post post, IFormFile myfile)
        {
            if (id != post.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    post.ImageUrl = await UserFile.UploadeNewImageAsync(post.ImageUrl,
            myfile, _environment.WebRootPath, Properties.Resources.ImgFolder, 100, 100);

                    _context.Update(post);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", post.CategoryId);
            ViewData["UserId"] = new SelectList(_context.ApplicationUser, "Id", "Id", post.UserId);
            return View(post);
        }

        // GET: Posts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.Category)
                .Include(p => p.User)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await _context.Posts.SingleOrDefaultAsync(m => m.Id == id);
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(int id)
        {
            return _context.Posts.Any(e => e.Id == id);
        }
    }
}
