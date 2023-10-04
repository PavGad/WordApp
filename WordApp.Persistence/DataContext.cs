using Microsoft.EntityFrameworkCore;
using WordApp.Persistence.Configuration;
using WordApp.Persistence.Models;

namespace WordApp.Persistence
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            Database.Migrate();
        }
        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Complaint> Complaints { get; set; }
        public DbSet<ComplaintReason> ComplaintReasons { get; set; }
        public DbSet<UserWord> UserWords { get; set; }
        public DbSet<Word> Words { get; set; }
        public DbSet<WordSet> WordSets { get; set; }
        public DbSet<ProposedWord> ProposedWords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new RefreshTokenConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new ComplaintConfiguration());
            modelBuilder.ApplyConfiguration(new ComplaintReasonConfiguration());
            modelBuilder.ApplyConfiguration(new WordConfiguration());
            modelBuilder.ApplyConfiguration(new WordSetConfiguration());
            modelBuilder.ApplyConfiguration(new UserWordConfiguration());
            modelBuilder.ApplyConfiguration(new ProposedWordConfiguration());
        }
    }
}
