using Blog.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Blog.Data
{
    public class ApplicationDbContext : IdentityDbContext<BlogUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { 
        }

        public virtual DbSet<BlogPost> BlogPlosts { get; set; } = default!;
        public virtual DbSet<Tag> Tags { get; set; } = default!;
        public virtual DbSet<Category> Catergories { get; set; } = default!;
        public virtual DbSet<Comment> Comments { get; set; } = default!;



    }

}