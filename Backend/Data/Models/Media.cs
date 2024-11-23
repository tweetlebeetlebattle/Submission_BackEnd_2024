using Backend.Data.ExtendedModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Data.Models
{
    public class Media
    {
        [Key]
        [StringLength(250)]
        public string MediaId { get; set; }  // UUID

        [StringLength(255)]
        public string? PictureUrl { get; set; }  

        [StringLength(255)]
        public string? TextUrl { get; set; }  
        [Required]
        [StringLength(450)]
        [ForeignKey("ApplicationUser")]
        public string ApplicationUserId { get; set; }  

        [Required]
        public virtual ApplicationUser ApplicationUser { get; set; }

    }
}
