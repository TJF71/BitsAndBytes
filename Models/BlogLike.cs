using System.ComponentModel.DataAnnotations;

namespace Blog.Models
{
    public class BlogLike
    {
        public int Id { get; set; } 
        public int BlogPostId { get; set; }
        public bool IsLiked { get; set; }        
        [Required]
        public string? BlogUserId { get; set; }
    
        public virtual BlogPost? BlogPost { get; set; } 
        public virtual BlogUser? BlogUser { get; set; }
    
    }
}


