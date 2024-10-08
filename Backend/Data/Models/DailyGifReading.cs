using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Data.Models
{
    public class DailyGifReading
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } 

        public float? DailyWaveMax { get; set; }
        public float? DailyWaveMin { get; set; }
        public float? DailyWaveAvg { get; set; }
        [ForeignKey("Units")]
        public int? WaveUnitId { get; set; }
        public virtual Units? WaveUnit { get; set; }  

        [Required]
        public DateTime Date { get; set; }  

        [Required]
        [ForeignKey("Locations")]
        public int LocationId { get; set; }
        [Required]
        public virtual Locations Location { get; set; } 
    }
}
