using Blog.Data;
using Blog.Models;
using Blog.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace Blog.Services
{
    public class BlogServices : IBlogServices
    {

        private readonly ApplicationDbContext _context;


        public BlogServices(ApplicationDbContext context)
        {
            _context = context;
        }


        //GET: BlogPosts
        public async Task<IEnumerable<BlogPost>> GetAllBlogPostsAsync()
        {
            try
            {

                IEnumerable<BlogPost> blogPosts = await _context.BlogPosts.Include(b => b.Category).ToListAsync();

                return blogPosts;

            }
            catch (Exception)
            {

                throw;
            }
        }


        // had to add lines below to fix error in View
        //private IActionResult View(List<BlogPost> blogPosts)
        //{
        //    throw new NotImplementedException();
        //}



    }
}
