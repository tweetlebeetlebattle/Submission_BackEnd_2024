using Backend.Data.ExtendedModel;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Data.Models
{
    public class TrainingComment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CommentId { get; set; } 

        [Required]
        [ForeignKey("TrainingBlog")]
        public int ParentBlogId { get; set; }  
        [Required]
        public virtual TrainingBlog ParentBlog { get; set; }  

        [Required]
        [StringLength(450)]
        [ForeignKey("ApplicationUser")]
        public string ApplicationUserId { get; set; }  // Matches .NET Identity User ID type

        [Required]
        public virtual ApplicationUser ApplicationUser { get; set; }

        [Required]
        public DateTime Time { get; set; } = DateTime.UtcNow; 

        [Required]
        [ForeignKey("Media")]
        public string MediaId { get; set; }
        [Required]
        public virtual Media Media { get; set; }

        public bool? ApprovedStatus { get; set; } = false;
    }
}
