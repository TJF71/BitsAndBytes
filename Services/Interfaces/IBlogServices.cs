using Blog.Data;
using Blog.Models;
using Microsoft.AspNetCore.Mvc;
using System.Composition.Convention;

namespace Blog.Services.Interfaces
{
    public interface IBlogServices
    {
        public Task AddTagAsync(IEnumerable<int> tagId, int blogPostId);

        public Task RemoveTagAsync(int blogPostId);

        public Task AddTagsToBlogPostAsync(IEnumerable<string>? tags, int? blogPostId);
        public Task<IEnumerable<Tag>> GetTagsAsync();


        public Task<IEnumerable<BlogPost>> GetAllBlogPostsForAuthorAsync();
        public Task<IEnumerable<BlogPost>> GetAllBlogPostsAsync();

        public Task<IEnumerable<BlogPost>> GetAllDeletedBlogPostsAsync();

        public Task<BlogPost> GetBlogPostBySlugAsync(string? slug);

        public Task<BlogPost> GetBlogPostByIdAsync(int? id);

        public Task<IEnumerable<BlogPost>> GetBlogPostByCategoryId(int? id);

        public Task<Category> GetCategoryByIdAsync(int? id);

        public Task<IEnumerable<BlogPost>> GetPopularBlogPostsAsync();

        public Task CreateBlogPostAsync(BlogPost blogPost);

        //edit
        public Task UpdateBlogPostAsync(BlogPost blogPost);

        public Task<IEnumerable<Category>> GetCategoriesAsync();

        public IEnumerable<BlogPost> SearchBlogPosts(string searchString);

        public Task<bool> IsValidSlugAsnyc(string? title, int? blogPostId);

        public Task<IEnumerable<BlogPost>> GetBlogPostByTagIdAsync(int? tagId);

    }

}