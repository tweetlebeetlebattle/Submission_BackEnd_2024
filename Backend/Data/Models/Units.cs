using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Data.Models
{
    public class Units
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UnitId { get; set; }

        [Required]
        [StringLength(20)]
        [Column(TypeName = "nvarchar(20)")]
        public string UnitName { get; set; }
    }
}
