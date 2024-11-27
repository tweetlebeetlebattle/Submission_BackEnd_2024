namespace Backend.DTO.RequestResponseDTOs.Shared
{
    public class CreateNewCommentDto
    {
        public string BlogId { get; set; }
        public string Text { get; set; }
        public IFormFile Image { get; set; }
    }

}
