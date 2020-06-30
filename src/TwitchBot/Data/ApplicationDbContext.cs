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

            // TODO: Type in your commands here!
            builder.Entity<Command>()
                .HasData(
                    new Command() { Id = 1, Type = CommandType.Text, Name = "test", Output = "We're working today... on me! The Twitchbot. :-)", SoundFile = "" }
                );
        }
    }
}