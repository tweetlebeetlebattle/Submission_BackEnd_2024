using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Data.Models
{
    public class TrainingSetsLog
    {
        [Key]
        [StringLength(250)]
        public string WorkSetId { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [ForeignKey("TrainingLog")]
        public string TrainingLogId { get; set; }  // UUID
        [Required]
        public virtual TrainingLog TrainingLog { get; set; }  

        [Required]
        public int DoneSetCount { get; set; } 

        [Required]
        public int DoneRepCount { get; set; } 
        [ForeignKey("Media")]
        public string? MediaId { get; set; }  
        public virtual Media? Media { get; set; }  

        [Required]
        public DateTime Time { get; set; }  
    }
}
