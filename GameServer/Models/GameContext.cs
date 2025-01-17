using Microsoft.EntityFrameworkCore;

namespace GameServer.Models
{
    public class GameContext : DbContext
    {
        public GameContext(DbContextOptions<GameContext> options) : base(options) { }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Item> Items { get; set; } = null!;

        // Item 클래스의 기본 키(UserId, X, Y) 설정
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Item>()
                .HasKey(item => new { item.UserId, item.X, item.Y });

            base.OnModelCreating(modelBuilder); // 부모 클래스의 기본 설정 호출
        }
    }
}
