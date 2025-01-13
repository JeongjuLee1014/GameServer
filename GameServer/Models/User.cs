namespace GameServer.Models
{
    public class User
    {
        public required string Id { get; set; }
        public required string NickName { get; set; }
        public required string SessionId { get; set; }
        public int numCoins {  get; set; }
        public int numStars {  get; set; }
        public int numEnergies { get; set; }
    }
}