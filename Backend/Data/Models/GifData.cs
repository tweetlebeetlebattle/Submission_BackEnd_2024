﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Data.Models
{
    public class GifData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }  
        public float? WaveRead { get; set; }
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
