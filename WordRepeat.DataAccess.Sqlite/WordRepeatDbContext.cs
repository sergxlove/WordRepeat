using Microsoft.EntityFrameworkCore;
using WordRepeat.DataAccess.Sqlite.Configurations;
using WordRepeat.DataAccess.Sqlite.Models;

namespace WordRepeat.DataAccess.Sqlite
{
    public class WordRepeatDbContext : DbContext
    {
        public WordRepeatDbContext(DbContextOptions<WordRepeatDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        public DbSet<HistoryAddEntity> HistoryAddTable { get; set; }
        public DbSet<HistoryTrainEntity> HistoryTrainTable { get; set; }
        public DbSet<HistoryTypesEntity> HistoryTypesTable { get; set; }
        public DbSet<WordsPairEntity> WordPairsTable { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new HistoryAddConfiguration());
            modelBuilder.ApplyConfiguration(new HistoryTrainConfiguration());
            modelBuilder.ApplyConfiguration(new HistoryTypesConfiguration());
            modelBuilder.ApplyConfiguration(new WordsPairConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
