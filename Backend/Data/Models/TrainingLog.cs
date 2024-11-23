using Backend.Data.ExtendedModel;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Data.Models
{
    public class TrainingLog
    {
        [Key]
        [StringLength(250)]
        public string TrainingLogId { get; set; }  // UUID

        [Required]
        [StringLength(450)]
        [ForeignKey("ApplicationUser")]
        public string ApplicationUserId { get; set; }  

        [Required]
        public virtual ApplicationUser ApplicationUser { get; set; }

        [Required]
        [StringLength(50)]
        public string ExerciseName { get; set; }  

        [Required]
        public float TargetWorkingWeight { get; set; }  

        [ForeignKey("Unit")]
        public int? UnitId { get; set; }
        public virtual TrainingUnits? Unit { get; set; }  

        [Required]
        public int TargetSetCount { get; set; }  

        [Required]
        public int TargetRepsCount { get; set; }  

        [Required]
        public DateTime Time { get; set; }  
        [Required]
        public bool IsPublic { get; set; } = false;  // Indicates whether the data is public (default is false)
    }
}
