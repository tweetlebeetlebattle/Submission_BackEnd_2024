namespace Backend.DTO.RequestResponseDTOs.Shared
{
    public class CreateNewBlogDto
    {
        public string Text { get; set; }
        public IFormFile Image { get; set; }
        public string DateTimestamp { get; set; } 
    }
}
