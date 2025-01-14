namespace GameServer.Models
{
    public class User
    {
        public required string Id { get; set; }
        public required string NickName { get; set; }
        public required string SessionId { get; set; }
        
        public int NumCoins {  get; set; }
        public int NumStars {  get; set; }
        public int NumEnergies { get; set; }
    }
}