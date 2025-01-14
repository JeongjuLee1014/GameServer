namespace GameServer.Models
{
    public class UserDTO
    {
        public required string NickName { get; set; }
        public required string SessionId { get; set; }

        public int NumCoins { get; set; }
        public int NumStars { get; set; }
        public int NumEnergies { get; set; }
    }
}
