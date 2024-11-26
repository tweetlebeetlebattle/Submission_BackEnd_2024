namespace Backend.DTO.RequestResponseDTOs.Shared
{
    public class BlogWithComments
    {
        public string BlogId { get; set; }
        public string ApplicationUserName { get; set; }
        public string MediaTextUrl { get; set; }
        public string MediaPictureUrl { get; set; }
        public DateTime Time { get; set; }
        public List<CommentDto> Comments { get; set; }
    }

    public class CommentDto
    {
        public string ApplicationUserName { get; set; }
        public string MediaTextUrl { get; set; }
        public string MediaPictureUrl { get; set; }
        public DateTime Time { get; set; }
    }
}
