namespace Backend.DTO
{
    public class CreateNewCommentDto
    {
        public string BlogId { get; set; }
        public string Text { get; set; }
        public IFormFile Image { get; set; }
        public string UserId { get; set; }
        public string DateTimestamp { get; set; }
    }
}
