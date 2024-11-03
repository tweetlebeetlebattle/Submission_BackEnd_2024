using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Data.Models
{
    public class GlassStormIoData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } 
        public float? WaveRead { get; set; }
        [ForeignKey("Units")]
        public int? WaveUnitId { get; set; }
        public virtual Units? WaveUnit { get; set; }

        public float? TempRead { get; set; }
        [ForeignKey("Units")]
        public int? TempUnitId { get; set; }
        public virtual Units? TempUnit { get; set; }

        public float? WindSpeedIndex { get; set; }
        [ForeignKey("Units")]
        public int? WindSpeedUnitId { get; set; }
        public virtual Units? WindSpeedUnit { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        [ForeignKey("Locations")]
        public int LocationId { get; set; }
        [Required]
        public virtual Locations Location { get; set; }
    }
}
