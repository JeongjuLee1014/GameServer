namespace GameServer.Models
{
    public class ItemDTO
    {
        public required string State { get; set; }
        public required string Type { get; set; }
        public required string Name { get; set; }
        public int Level { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }
}
