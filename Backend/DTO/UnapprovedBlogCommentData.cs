namespace Backend.DTO
{
    public class UnapprovedBlogCommentData
    {
        public List<UnapprovedBlogComment> UnapprovedData { get; set; }
    }
    public class UnapprovedBlogComment
    {
        public string Id { get; set; }
        public string TextUrl { get; set; }
        public string PictureUrl { get; set; }

    }
}