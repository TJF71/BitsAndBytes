using Blog.Data;
using Blog.Models;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Services.Interfaces
{
    public interface IBlogServices
    {
        
        public Task<IEnumerable<BlogPost>> GetAllBlogPostsAsync();

        public Task<IEnumerable<BlogPost>> GetAllDeletedBlogPostsAsync();

        public Task<BlogPost> GetBlogPostByIdAsync(int? id);

        public Task CreateBlogPostAsync(BlogPost blogPost);

        //edit
        public Task UpdateBlogPostAsync(BlogPost blogPost);

        public Task<IEnumerable<Category>> GetCategoriesAsync();

    }

}