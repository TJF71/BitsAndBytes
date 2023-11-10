using System.ComponentModel.DataAnnotations;

namespace Blog.Models
{
    public class Comment
    {

        
        private DateTimeOffset _created;
        private DateTimeOffset? _updated;
        
        // Primary Key
        public int Id { get; set; }

        // Foreign Keys
        [Required]
        public string? BlogPostId { get; set; }

        [Required]
        public string? AuthorId { get; set; }

        [Required]
        [Display(Name = "Body")]
        [StringLength(5000, ErrorMessage = "The {0} must be at least {2} and max {1} characters long.", MinimumLength = 2)]
        public string? Body { get; set; }


        public DateTimeOffset Created { get { return _created; } set { _created = value.ToUniversalTime(); } }

        public DateTimeOffset? Updated
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

        [Display(Name = "UpdatedReason")]
        [StringLength(200 , ErrorMessage = "The {0} must be at least {2} and at most {1} characters long.", MinimumLength = 2)]
        public string? UpdatedReason { get; set; }


        // Navigation Properties
        public virtual BlogPost? BlogPost { get; set; }

        public virtual BlogUser? Author { get; set; } 

    }
}
