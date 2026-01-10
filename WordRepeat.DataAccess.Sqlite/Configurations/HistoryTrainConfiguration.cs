using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WordRepeat.DataAccess.Sqlite.Models;

namespace WordRepeat.DataAccess.Sqlite.Configurations
{
    public class HistoryTrainConfiguration : IEntityTypeConfiguration<HistoryTrainEntity>
    {
        public void Configure(EntityTypeBuilder<HistoryTrainEntity> builder)
        {
            builder.ToTable("historytrain");
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Name)
                .IsRequired();
            builder.Property(a => a.Result)
                .IsRequired();
            builder.Property(a => a.Total)
                .IsRequired();
            builder.Property(a => a.Date)
                .IsRequired();
        }
    }
}
