using QuestSolve.models;
using Microsoft.EntityFrameworkCore;

namespace QuestSolve.Config
{
    //Конфигурация на базата данни на апликацията
    public class ForumDbContext : DbContext
    {
        public ForumDbContext(DbContextOptions<ForumDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Response> Responses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Съдържа информация за сървъра, базата данни, потребителя и паролата.
                optionsBuilder.UseMySql("Server=localhost;Database=QuestSolve;User Id=root;Password=root;",
                    new MySqlServerVersion(new Version(8, 0, 21)));
            }
        }
    }
}