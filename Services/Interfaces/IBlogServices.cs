using Blog.Models;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Services.Interfaces
{
    public interface IBlogServices
    {

        public Task<IEnumerable<BlogPost>> GetAllBlogPostsAsync();

    }
}
