namespace GameServer.Models
{
    public class UserDTO
    {
        public required string nickName { get; set; }
        public required string sessionId { get; set; }

        public int numCoins { get; set; }
        public int numStars { get; set; }
        public int numEnergies { get; set; }
    }
}
