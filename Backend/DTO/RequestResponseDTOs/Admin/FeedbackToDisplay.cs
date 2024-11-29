namespace Backend.DTO.RequestResponseDTOs.Admin
{
    public class FeedbacksToDisplay
    {
        public List<FeedbackToDisplay> Feedbacks { get; set; }
    }
    public class FeedbackToDisplay
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string LocationName { get; set; }
        public string Date {  get; set; }
        public float? WaveRead { get; set; }
        public string? WaveUnitName { get; set; }
        public float? TempRead { get; set; }
        public string? TempUnitName { get; set; }
        public float? windSpeedRead { get; set; }
        public string? WindSpeedUnitName { get; set; }
        public string? ImageUrl { get; set; }
        public string? TextUrl { get; set; }
    }
}
