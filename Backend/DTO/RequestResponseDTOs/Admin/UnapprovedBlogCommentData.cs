namespace Backend.DTO.RequestResponseDTOs.Admin
{
    public class UnapprovedBlogComment
    {
        public string Id { get; set; }
        public string TextUrl { get; set; }
        public string Username { get; set; }
        public string PictureUrl { get; set; }
        public DateTime TimeOfPosting {  get; set; }
    }

    public class UnapprovedBlogCommentData
    {
        public List<UnapprovedBlogComment> UnapprovedData { get; set; }
    }
}
