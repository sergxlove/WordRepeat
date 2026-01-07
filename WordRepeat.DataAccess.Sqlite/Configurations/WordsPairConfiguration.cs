using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WordRepeat.DataAccess.Sqlite.Models;

namespace WordRepeat.DataAccess.Sqlite.Configurations
{
    public class WordsPairConfiguration : IEntityTypeConfiguration<WordsPairEntity>
    {
        public void Configure(EntityTypeBuilder<WordsPairEntity> builder)
        {
            throw new NotImplementedException();
        }
    }
}
