using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WordRepeat.DataAccess.Sqlite.Models;

namespace WordRepeat.DataAccess.Sqlite.Configurations
{
    public class HistoryAddConfiguration : IEntityTypeConfiguration<HistoryAddEntity>
    {
        public void Configure(EntityTypeBuilder<HistoryAddEntity> builder)
        {
            builder.ToTable("historyadd");
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Name)
                .IsRequired();
            builder.Property(a => a.Date)
                .IsRequired();
            builder.Property(a => a.CountAdd)
                .IsRequired();
        }
    }
}
