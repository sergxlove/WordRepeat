using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WordRepeat.DataAccess.Sqlite.Models;

namespace WordRepeat.DataAccess.Sqlite.Configurations
{
    public class WordsPairConfiguration : IEntityTypeConfiguration<WordsPairEntity>
    {
        public void Configure(EntityTypeBuilder<WordsPairEntity> builder)
        {
            builder.ToTable("words");
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Word)
                .IsRequired();
            builder.Property(a => a.Tranclate)
                .IsRequired();
        }
    }
}
