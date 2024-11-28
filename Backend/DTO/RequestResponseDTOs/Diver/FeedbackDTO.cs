using System.ComponentModel.DataAnnotations;

namespace Backend.DTO.RequestResponseDTOs.Diver
{
    public class FeedbackDTO
    {
        public string LocationName { get; set; }
        public string? Coordinates { get; set; }
        public float? WaveRead { get; set; }
        public int? WaveUnitId { get; set; } 

        public float? TempRead { get; set; }
        public int? TempUnitId { get; set; } 

        public float? WindSpeedIndex { get; set; }
        public int? WindSpeedUnitId { get; set; }
        public IFormFile? Image { get; set; }
        public string? Text { get; set; }
    }
}
