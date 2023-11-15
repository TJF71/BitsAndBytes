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

        public async Task<BlogPost> GetBlogPostByIdAsync(int? id)
        {
            try
            {
                BlogPost? blogPost = await _context.BlogPosts
                                          .Include(b => b.Category)
                                          .Include(b => b.Comments)
                                          .ThenInclude(b => b.Author)
                                          .FirstOrDefaultAsync(m => m.Id == id && m.IsDeleted == false && m.IsPublished == true);
                return blogPost!;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<BlogPost>> GetAllBlogPostsAsync()
        {
            try
            {

                IEnumerable<BlogPost> blogPosts = await _context.BlogPosts
                                                        .Where(b => b.IsDeleted == false)
                                                        .Include(b => b.Category)
                                                        .ToListAsync();

                return blogPosts;

            }
            catch (Exception)
            {

                throw;
            }
        }


        public async Task CreateBlogPostAsync(BlogPost blogPost)
        {
            try
            {
                _context.Add(blogPost);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }



        //GET: BlogPost details


        public async Task  UpdateBlogPostAsync(BlogPost blogPost)
        {
            try
            {
                _context.Update(blogPost);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
