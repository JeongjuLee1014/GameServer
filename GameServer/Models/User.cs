namespace GameServer.Models
{
    public class User
    {
        public long Id { get; set; }
        public required string NickName { get; set; }
        public required string SessionId { get; set; }
    }
}