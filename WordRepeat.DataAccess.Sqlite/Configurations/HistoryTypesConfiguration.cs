using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WordRepeat.DataAccess.Sqlite.Models;

namespace WordRepeat.DataAccess.Sqlite.Configurations
{
    public class HistoryTypesConfiguration : IEntityTypeConfiguration<HistoryTypesEntity>
    {
        public void Configure(EntityTypeBuilder<HistoryTypesEntity> builder)
        {
            builder.ToTable("historytypes");
            builder.HasKey(a => a.Id);
            builder.Property(a => a.NameType)
                .IsRequired();
        }
    }
}
