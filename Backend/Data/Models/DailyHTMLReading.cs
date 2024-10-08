using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Data.Models
{
    public class DailyHTMLReading
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DailyHTMLReadingId { get; set; }  // Primary key with auto-increment

        public float? DailyWaveMax { get; set; }
        public float? DailyWaveMin { get; set; }
        public float? DailyWaveAvg { get; set; }
        [ForeignKey("Units")]
        public int? WaveUnitId { get; set; }
        public virtual Units? WaveUnit { get; set; } 

        public float? DailyTempMax { get; set; }
        public float? DailyTempMin { get; set; }
        public float? DailyTempAvg { get; set; }
        [ForeignKey("TempUnit")]
        public int? TempUnitId { get; set; }
        public virtual Units? TempUnit { get; set; }  

        [Required]
        public DateTime Date { get; set; }  // Use DateTime for the DATE type

        [Required]
        [ForeignKey("Locations")]
        public int LocationId { get; set; }
        [Required]
        public virtual Locations Location { get; set; } 
    }
}
