using Backend.Data.ExtendedModel;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Data.Models
{
    public class UniversalReading
    {
        [Key, Column(Order = 0)]
        [Required]
        [StringLength(450)]
        [ForeignKey("ApplicationUser")]
        public string ApplicationUserId { get; set; }  // Matches .NET Identity User ID type

        [Required]
        public virtual ApplicationUser ApplicationUser { get; set; }

        [Required]
        [StringLength(50)]
        public string Title { get; set; } 

        [Required]
        public float Reading { get; set; }  

        [ForeignKey("Units")]
        [Required]
        public int UnitId { get; set; }
        [Required]
        public virtual TrainingUnits Unit { get; set; } 

        [Required]
        public DateTime Time { get; set; }  
        [Required]
        public bool IsPublic { get; set; } = false;  // Indicates whether the data is public (default is false)
    }
}
