using Microsoft.EntityFrameworkCore;

namespace TwitchBot.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionbuilder)
        {
            optionbuilder.UseSqlite(@"Data Source=.\TwitchBotDB.db");
        }

        public DbSet<Command> Command { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Command seeding.
            builder.Entity<Command>()
                .HasData(
                    new Command() { Id = 1, Type = CommandType.Text, Name = "test", Output = "This is a sample test command. :-)", SoundFile = "" }
                );
        }
    }
}