using Backend.Data.ExtendedModel;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Data.Models
{
    public class TrainingBlog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string BlogId { get; set; } = Guid.NewGuid().ToString();

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
        [Required]
        public virtual List<TrainingComment> TrainingComments { get; set; }
    }
}
