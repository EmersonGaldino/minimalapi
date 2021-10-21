using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TodoList.Models;

namespace TodoList.Repository.Base
{
    public class AppDbContext : DbContext
    {
        public DbSet<Todo> Todo { get; set; }
        public DbSet<User> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlite("DataSource=Repository/app.db; Cache=Shared")
            .EnableSensitiveDataLogging().UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()));


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Todo>()
            .HasOne(x => x.User)
            .WithMany(c => c.Todo)
            .HasForeignKey(u => u.UserId);

            modelBuilder.Entity<User>()
            .HasMany<Todo>()
            .WithOne(c => c.User)
            .HasForeignKey(x => x.TodoId);
        }
    }
}