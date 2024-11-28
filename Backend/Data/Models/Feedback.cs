using Backend.Data.ExtendedModel;
using Backend.Data.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Data.Models
{
    public class Feedback
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [StringLength(450)]
        [ForeignKey("ApplicationUser")]
        public string ApplicationUserId { get; set; }  

        [Required]
        public virtual ApplicationUser ApplicationUser { get; set; }
        [Required]
        [ForeignKey("Locations")]
        public int LocationId { get; set; }
        [Required]
        public virtual Locations Location { get; set; }

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
        public DateTime Time { get; set; }
        [ForeignKey("Media")]
        public string? MediaId { get; set; }
        public virtual Media? Media { get; set; }
    }
}
