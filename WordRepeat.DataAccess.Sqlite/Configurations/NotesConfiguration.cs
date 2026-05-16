using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WordRepeat.DataAccess.Sqlite.Models;

namespace WordRepeat.DataAccess.Sqlite.Configurations
{
    public class NotesConfiguration : IEntityTypeConfiguration<NotesEntity>
    {
        public void Configure(EntityTypeBuilder<NotesEntity> builder)
        {
            builder.ToTable("notes");
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Title)
                .IsRequired();
            builder.Property(a => a.Content)
                .IsRequired();
            builder.Property(a => a.DateUpdate)
                .IsRequired();
        }
    }
}
