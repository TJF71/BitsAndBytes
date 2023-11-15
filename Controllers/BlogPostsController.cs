using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Blog.Data;
using Blog.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Blog.Services.Interfaces;

namespace Blog.Controllers
{
    public class BlogPostsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<BlogUser> _userManager;
        private readonly IBlogServices _blogServices;
        //private readonly IImageService _imageService;
        //private readonly IEmailSender _emailService;

        public BlogPostsController(ApplicationDbContext context, UserManager<BlogUser> userManager, IBlogServices blogServices)

        {
            _context = context;
            _userManager = userManager;
            _blogServices = blogServices;
        }

        // GET: BlogPosts
        public async Task<IActionResult> Index()
        {
            IEnumerable<BlogPost> blogPosts = await _blogServices.GetAllBlogPostsAsync();

            return View(blogPosts);
        }

        // GET: BlogPosts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.BlogPosts == null)
            {
                return NotFound();
            }

            //GetBlogPostByIdAsync(int? id)

            BlogPost? blogPosts = await _blogServices.GetBlogPostByIdAsync(id);


            if (blogPosts == null)
            {
                return NotFound();
            }

            return View(blogPosts);
        }



        // GET: BlogPosts/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        // POST: BlogPosts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CategoryId,Title,Abstract,Content,CreatedDate,UpdatedDate,Slug,IsDeleted,IsPublished,ImageData,ImageType")] BlogPost blogPost)
        {
            if (ModelState.IsValid)
            {
                //_context.Add(blogPost);
                //await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));

                blogPost.CreatedDate = DateTime.Now;

                await _blogServices.CreateBlogPostAsync(blogPost);
                return RedirectToAction("Index");   

            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", blogPost.CategoryId);
            return View(blogPost);
        }



        // GET: BlogPosts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.BlogPosts == null)
            {
                return NotFound();
            }

            var blogPost = await _context.BlogPosts.FindAsync(id);
            if (blogPost == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", blogPost.CategoryId);
            return View(blogPost);
        }
        // POST: BlogPosts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CategoryId,Title,Abstract,Content,CreatedDate,UpdatedDate,Slug,IsDeleted,IsPublished,ImageData,ImageType")] BlogPost blogPost)
        {
            if (id != blogPost.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await BlogPostExists(blogPost.Id))
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", blogPost.CategoryId);
            return View(blogPost);
        }

        // GET: BlogPosts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.BlogPosts == null)
            {
                return NotFound();
            }

            var blogPost = await _blogServices.GetBlogPostByIdAsync(id);


            if (blogPost == null)
            {
                return NotFound();
            }

            return View(blogPost);
        }

        // POST: BlogPosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.BlogPosts == null)
            {
                return Problem("Entity set 'ApplicationDbContext.BlogPosts'  is null.");
            }
            var blogPost = await _context.BlogPosts.FindAsync(id);
            if (blogPost != null)
            {
               blogPost.IsDeleted = true;
                await _blogServices.UpdateBlogPostAsync(blogPost);


            }
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> BlogPostExists(int id)
        {
            return ((await _blogServices.GetAllBlogPostsAsync()).Any(e => e.Id == id));
        }
    }
}
