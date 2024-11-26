using Backend.Data.ExtendedModel;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Data.Models
{
    public class SeaComment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string CommentId { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [ForeignKey("SeaBlog")]
        public string ParentBlogId { get; set; }
        [Required]
        public virtual SeaBlog ParentBlog { get; set; }  

        [Required]
        [StringLength(450)]
        [ForeignKey("ApplicationUser")]
        public string ApplicationUserId { get; set; }  

        [Required]
        public virtual ApplicationUser ApplicationUser { get; set; }

        [Required]
        public DateTime Time { get; set; } = DateTime.UtcNow; 

        [Required]
        [ForeignKey("Media")]
        public string MediaId { get; set; }
        [Required]
        public virtual Media Media { get; set; }
        [Required]
        public bool ApprovedStatus { get; set; } = false; 
    }
}
