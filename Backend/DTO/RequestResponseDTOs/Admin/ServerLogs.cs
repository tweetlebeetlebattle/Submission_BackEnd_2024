namespace Backend.DTO.RequestResponseDTOs.Admin
{
    public class ServerLogs
    {
        public List<ServerLog> _ServerLogs { get; set; }
    }
    public class ServerLog
    {
        public string Id { get; set; }
        public string StatusLog { get; set; }    
        public string Time { get; set; }
    }
}
