namespace GameServer.Models
{
    public class Item
    {
        public required string UserId { get; set; }
        public required string State { get; set; } // 보드 위: placed, 창고: stored, 없음: none
        public required string Type { get; set; } // 재화: goods, 생성: generate, 합성: synthesize, 건물수리: repair
        public required string Name { get; set; } // 아이템명
        public int Level { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }
}
