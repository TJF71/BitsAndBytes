using Microsoft.AspNetCore.Http.HttpResults;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Blog.Models
{
   
  
    public class BlogPost
    {

        private DateTimeOffset _created;
        private DateTimeOffset? _updated;
        
        //Primary Key
        public int Id { get; set; }

        //Foreing Key
        [Required]
        public int CategoryId { get; set; }

        [Required]
        [Display(Name = "Title")]
        [StringLength(200, ErrorMessage = "The {0} must be at least {2} and max {1} characters long.", MinimumLength = 2)]
        public string? Title { get; set; }

        [Display(Name = "Abstract")]
        [StringLength(600, ErrorMessage = "The {0} must be at least {2} and max {1} characters long.", MinimumLength = 2)]
        public string? Abstract { get; set; }

        [Required]
        [Display(Name = "Content")]
        public string? Content { get; set; }

        [Required]
        public DateTimeOffset CreatedDate { get { return _created; } set { _created = value.ToUniversalTime(); } }

        public DateTimeOffset? UpdatedDate
        {
            get => _updated;
            set
            {
                if (value.HasValue)
                {
                    _updated = value.Value.ToUniversalTime();
                }
            }
        }

        [Required]
        public string? Slug { get; set; } // "an-interesting-blog-post

        public bool IsDeleted { get; set; }

        public bool IsPublished { get; set; }

        [NotMapped]
        public IFormFile? ImageFile { get; set; }

        public byte[]? ImageData { get; set; }

        public string? ImageType { get; set; }


        // Navigation Properties

        public virtual Category? Category { get; set; }

        public virtual ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();

        public virtual ICollection<Tag> Tags { get; set; } = new HashSet<Tag>();

        public virtual ICollection<BlogLike> Likes { get; set; } = new HashSet<BlogLike>();

    }
}
