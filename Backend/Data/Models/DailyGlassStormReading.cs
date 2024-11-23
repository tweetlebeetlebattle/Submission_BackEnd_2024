using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Data.Models
{
    public class DailyGlassStormReading
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

        public float? DailyTempMax { get; set; }
        public float? DailyTempMin { get; set; }
        public float? DailyTempAvg { get; set; }
        [ForeignKey("Units")]
        public int? TempUnitId { get; set; }
        public virtual Units? TempUnit { get; set; } 

        public float? DailyWindMax { get; set; }
        public float? DailyWindMin { get; set; }
        public float? DailyWindAvg { get; set; }
        [ForeignKey("Units")]
        public int? WindUnitId { get; set; }
        public virtual Units? WindUnit { get; set; } 

        [Required]
        public DateTime Date { get; set; } 

        [Required]
        [ForeignKey("Locations")]
        public int LocationId { get; set; }
        public virtual Locations Location { get; set; }  
    }
}
