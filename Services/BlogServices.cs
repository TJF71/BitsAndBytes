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


        public async Task<IEnumerable<BlogPost>> GetAllDeletedBlogPostsAsync()
        {
            try
            {

                IEnumerable<BlogPost> blogPosts = await _context.BlogPosts
                                                        .Where(b => b.IsDeleted == true)
                                                        .Include(b => b.Category)
                                                        .ToListAsync();

                return blogPosts;

            }
            catch (Exception)
            {

                throw;
            }
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





        public async Task<IEnumerable<BlogPost>> GetBlogPostByCategoryId(int? id)
        {
            try
            {
                IEnumerable<BlogPost> blogPosts = await _context.BlogPosts
                                                        .Where(b => b.IsDeleted == false && b.IsPublished == true && b.CategoryId == id)
                                                        .Include(b => b.Category)
                                                        .Include(b => b.Comments)
                                                        .Include(b => b.Tags)
                                                        .OrderByDescending(b => b.Comments.Count)
                                                        .ToListAsync();
                return blogPosts;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Category> GetCategoryByIdAsync(int? Id)
        {
            try
            {
                Category? category = await _context.Categories
                                    .FirstOrDefaultAsync(category => category.Id == Id);                  
 
                return category!;
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
                                                        .Where(b => b.IsDeleted == false && b.IsPublished == true)
                                                        .Include(b => b.Category)
                                                        .Include(b => b.Comments)
                                                        .ToListAsync();

                return blogPosts;

            }
            catch (Exception)
            {

                throw;
            }
        }


        public async Task<IEnumerable<BlogPost>> GetAllBlogPostsForAuthorAsync()
        {
            try
            {

                IEnumerable<BlogPost> blogPosts = await _context.BlogPosts
                                                        .Include(b => b.Category)
                                                        .Include(b => b.Comments)
                                                        .ToListAsync();

                return blogPosts;

            }
            catch (Exception)
            {

                throw;
            }
        }


        public async Task<IEnumerable<BlogPost>> GetPopularBlogPostsAsync()
        {
            try
            {

                IEnumerable<BlogPost> blogPosts = await _context.BlogPosts
                                                        .Where(b => b.IsDeleted == false && b.IsPublished == true)
                                                        .Include(b => b.Category)
                                                        .Include(b => b.Comments)
                                                        .Include(b => b.Tags)
                                                        .OrderByDescending(b => b.Comments.Count)
                                                        .ToListAsync();

                return blogPosts;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<BlogPost> GetBlogPostBySlugAsync(string? slug)
        {
            try
            {
                BlogPost? blogPost = await _context.BlogPosts
                                          .Include(b => b.Category)
                                          .Include(b => b.Comments)
                                          .ThenInclude(b => b.Author)
                                          .FirstOrDefaultAsync(m => m.Slug == slug && m.IsDeleted == false && m.IsPublished == true);
                return blogPost!;
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


        public async Task UpdateBlogPostAsync(BlogPost blogPost)
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


        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            try
            {
                IEnumerable<Category> categories = await _context.Categories.ToListAsync();

                return categories;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IEnumerable<BlogPost> SearchBlogPosts(string searchString)  //called a method signature (the whole line)
        {
            try
            {
                searchString = searchString.Trim().ToLower();

                IEnumerable<BlogPost> blogPosts = _context.BlogPosts
                                                    .Where(b => b.IsPublished == true && b.IsDeleted == false)
                                                    .Where(b => b.Title!.ToLower().Contains(searchString)
                                                           || (!string.IsNullOrEmpty(b.Abstract) && b.Abstract.ToLower().Contains(searchString))
                                                           || b.Content!.ToLower().Contains(searchString)
                                                           || b.Tags.Any(t => t.Body!.ToLower().Contains(searchString))
                                                           || b.Category!.Name!.ToLower().Contains(searchString)
                                                           || b.Comments.Any(c => c.Body!.ToLower().Contains(searchString)
                                                                            || c.Author!.FirstName!.ToLower().Contains(searchString)
                                                                            || c.Author!.LastName!.ToLower().Contains(searchString))
                                                           )
                                                    .Include(b => b.Category)
                                                    .Include(b => b.Tags)
                                                    .Include(b => b.Comments).ThenInclude(c => c.Author)
                                                    .AsNoTracking()
                                                    .OrderByDescending(b => b.UpdatedDate ?? b.CreatedDate)
                                                    .AsEnumerable();

                return blogPosts;
            }
            catch (Exception)
            {
                return new List<BlogPost>();
                throw;
            }
        }

        public async Task<bool> IsValidSlugAsnyc(string? slug, int? blogPostId)
        {
            try
            {
                // This indicates that a new blog is being created
                if (blogPostId == null || blogPostId == 0)
                {
                    bool isSlug = !await _context.BlogPosts.AnyAsync(b => b.Slug == slug);

                    return isSlug;
                }
                else
                {
                    // Editing an existing BlogPost
                    BlogPost? blogPost = await _context.BlogPosts.AsNoTracking().FirstOrDefaultAsync(b => b.Id == blogPostId);

                    string? oldSlug = blogPost?.Slug;

                    if (!string.Equals(oldSlug, slug))
                    {
                        return !await _context.BlogPosts.AnyAsync(b => b.Id != blogPost!.Id && b.Slug == slug);
                    }
                }

                return true;
            }
            catch (Exception)
            {

                throw;
            }

        }

    }
}


