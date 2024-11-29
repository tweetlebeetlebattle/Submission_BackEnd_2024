using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Backend.DTO.RequestResponseDTOs.Diver
{
    public class FeedbackDTO
    {
        public string LocationName { get; set; }
        public string? Coordinates { get; set; }
        public float? WaveRead { get; set; }
        public string? WaveUnitId { get; set; }
        public float? TempRead { get; set; }
        public string? TempUnitId { get; set; }
        public float? windSpeedRead { get; set; }
        public string? WindSpeedUnitId { get; set; }
        public IFormFile? Image { get; set; }
        public string? Text { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"LocationName: {LocationName}");
            sb.AppendLine($"Coordinates: {Coordinates}");
            sb.AppendLine($"WaveRead: {WaveRead}");
            sb.AppendLine($"WaveUnitId: {WaveUnitId}");
            sb.AppendLine($"TempRead: {TempRead}");
            sb.AppendLine($"TempUnitId: {TempUnitId}");
            sb.AppendLine($"WindSpeedIndex: {windSpeedRead}");
            sb.AppendLine($"WindSpeedUnitId: {WindSpeedUnitId}");
            sb.AppendLine($"Text: {Text}");
            sb.AppendLine($"Image: {(Image != null ? Image.FileName : "No file uploaded")}");
            return sb.ToString();
        }
    }
}
