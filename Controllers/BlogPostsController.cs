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
using X.PagedList;
using X.PagedList.Web.Common;
using Blog.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace Blog.Controllers
{
    public class BlogPostsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<BlogUser> _userManager;
        private readonly IBlogServices _blogServices;
        private readonly IImageService _imageService;
        //   private readonly IEmailSender _emailService;

        public BlogPostsController(ApplicationDbContext context, UserManager<BlogUser> userManager, IBlogServices blogServices, IImageService imageService)

        {
            _context = context;
            _userManager = userManager;
            _blogServices = blogServices;
            _imageService = imageService;
        }

        // AdminArea

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AuthorArea(int? pageNum)
        {
            int pageSize = 3;
            int page = pageNum ?? 1;

            // relpaced GetBlogPostsAsync()
            IPagedList<BlogPost> blogPosts = await (await _blogServices.GetAllBlogPostsForAuthorAsync())
                                                                      .ToPagedListAsync(page, pageSize);

            return View(blogPosts);
        }



        [Authorize(Roles = "Admin")]
        // GET: BlogPosts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

    
            BlogPost? blogPost = await _blogServices.GetBlogPostByIdAsync(id);

            //var blogPost = await _blogService.GetBlogPostAsync(id);

            //    .FirstOrDefaultAsync(m => m.Id == id);

            if (blogPost == null)
            {
                return NotFound();
            }

            blogPost.IsDeleted = true;

            await _blogServices.UpdateBlogPostAsync(blogPost);

            return RedirectToAction(nameof(AuthorArea));
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Undelete(int? id)
        {
            // replaced GetBlogPostAsync(id) 
            var blogPost = await _blogServices.GetBlogPostByIdAsync(id);

            if (blogPost != null)
            {
                blogPost.IsPublished = false;

                await _blogServices.UpdateBlogPostAsync(blogPost);
            }

            return RedirectToAction(nameof(Index));
        }



        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Publish(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

           
            BlogPost? blogPost = await _blogServices.GetBlogPostByIdAsync(id);

            if (blogPost == null)
            {
                return NotFound();
            }

            blogPost.IsPublished = true;

            await _blogServices.UpdateBlogPostAsync(blogPost);

            return RedirectToAction(nameof(AuthorArea));
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Unpublish(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

          
            BlogPost? blogPost = await _blogServices.GetBlogPostByIdAsync(id);

            if (blogPost == null)
            {
                return NotFound();
            }

            blogPost.IsPublished = false;

            await _blogServices.UpdateBlogPostAsync(blogPost);

            return RedirectToAction(nameof(AuthorArea));
        }


        // GET: BlogPosts
        [AllowAnonymous]
        public async Task<IActionResult> Index(int? pageNum)
        {
            int pageSize = 4;
            int page = pageNum ?? 1;

            IPagedList<BlogPost> blogPosts = await (await _blogServices.GetAllBlogPostsAsync()).ToPagedListAsync(page, pageSize);
            return View(blogPosts);
        }


        // GET: BlogPosts Search Index
        [AllowAnonymous]
        public async Task<IActionResult> SearchIndex(string? searchString, int? pageNum)
        {
            if (string.IsNullOrWhiteSpace(searchString))
            {
                return RedirectToAction(nameof(Index));
            }

            int pageSize = 4;
            int page = pageNum ?? 1;


            IPagedList<BlogPost> blogPosts = await _blogServices.SearchBlogPosts(searchString).ToPagedListAsync(page, pageSize);

            ViewData["Search"] = searchString;

            return View(nameof(Index), blogPosts);

        }

        public async Task<IActionResult> CategoryIndex(int? categoryId, int? pageNum)
        {
            if (categoryId == null)
            {
                return RedirectToAction(nameof(Index));
            }

            int pageSize = 4;
            int page = pageNum ?? 1;

            
            IPagedList<BlogPost> blogPosts = await (await _blogServices.GetBlogPostByCategoryId(categoryId)).ToPagedListAsync(page, pageSize);

            ViewData["categoryId"] = categoryId;

            return View(nameof(Index), blogPosts);
        }



        // GET:  DeletedBlogPosts
        public async Task<IActionResult> DeletedBlogPosts()
        {
            IEnumerable<BlogPost> blogPosts = await _blogServices.GetAllDeletedBlogPostsAsync();

            return View(blogPosts);

        }


        //GET: BlogPosts/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(string? slug)
        {
            if (string.IsNullOrEmpty(slug))
            {
                return NotFound();
            }

            //GetBlogPostByIdAsync(int? id)

            BlogPost? blogPosts = await _blogServices.GetBlogPostBySlugAsync(slug); //GetBlogPostBySlugAsync


            if (blogPosts == null)
            {
                return NotFound();
            }

            return View(blogPosts);
        }



        // GET: BlogPosts/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        // POST: BlogPosts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Abstract,Content,IsPublished, CategoryId, ImageFile")] BlogPost blogPost)
        {
            ModelState.Remove("Slug");


            if (ModelState.IsValid)
            {
                string? newSlug = StringHelper.BlogPostSlug(blogPost.Title);

                if(!await _blogServices.IsValidSlugAsnyc(newSlug, blogPost.Id))
                {
                    ModelState.AddModelError("Title", "A similar title/Slug is already in use.");

                    ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", blogPost.CategoryId);
                    return View(blogPost);
                }

                blogPost.Slug = newSlug;
                blogPost.CreatedDate = DateTimeOffset.Now;

                if (blogPost.ImageFile != null)
                {
                    blogPost.ImageData = await _imageService.ConvertFileToByteArrayAsync(blogPost.ImageFile);
                    blogPost.ImageType = blogPost.ImageFile.ContentType;
                }


                await _blogServices.CreateBlogPostAsync(blogPost);

                return RedirectToAction("Index");   

            }
            // make service call
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", blogPost.CategoryId);
            return View(blogPost);
        }



        // GET: BlogPosts/Edit/5
        [Authorize(Roles = "Admin,Moderator")]
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
        [Authorize(Roles = "Admin,Moderator")]
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
        //[Authorize(Roles = "Admin")]
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null || _context.BlogPosts == null)
        //    {
        //        return NotFound();
        //    }

        //    var blogPost = await _blogServices.GetBlogPostByIdAsync(id);


        //    if (blogPost == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(blogPost);
        //}

        // POST: BlogPosts/Delete/5
        //[Authorize(Roles = "Admin")]
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    if (_context.BlogPosts == null)
        //    {
        //        return Problem("Entity set 'ApplicationDbContext.BlogPosts'  is null.");
        //    }
        //    var blogPost = await _context.BlogPosts.FindAsync(id);
        //    if (blogPost != null)
        //    {
        //       blogPost.IsDeleted = true;
        //        await _blogServices.UpdateBlogPostAsync(blogPost);


        //    }
        //    return RedirectToAction(nameof(Index));
        //}

        private async Task<bool> BlogPostExists(int id)
        {
            return ((await _blogServices.GetAllBlogPostsAsync()).Any(e => e.Id == id));
        }
    }
}
