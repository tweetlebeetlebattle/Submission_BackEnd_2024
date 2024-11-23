using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Data.Models
{
    public class DataFetchingLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } 

        [StringLength(250)]
        [Required]
        public string StatusLog { get; set; } 

        [Required]
        public DateTime Time { get; set; } = DateTime.UtcNow;  
    }
}
