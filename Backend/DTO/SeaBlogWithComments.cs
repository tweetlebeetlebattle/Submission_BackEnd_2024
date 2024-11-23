namespace Backend.DTO
{
    public class SeaBlogWithComments
    {
        public string ApplicationUserName { get; set; }
        public string MediaTextUrl { get; set; }
        public string MediaPictureUrl { get; set; }
        public DateTime Time { get; set; }
        public List<SeaCommentDto> Comments { get; set; }
    }

    public class SeaCommentDto
    {
        public string ApplicationUserName { get; set; }
        public string MediaTextUrl { get; set; }
        public string MediaPictureUrl { get; set; }
        public DateTime Time { get; set; }
    }
}
