using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WordRepeat.DataAccess.Sqlite.Models;

namespace WordRepeat.DataAccess.Sqlite.Configurations
{
    public class HistoryAddConfiguration : IEntityTypeConfiguration<HistoryAddEntity>
    {
        public void Configure(EntityTypeBuilder<HistoryAddEntity> builder)
        {
            throw new NotImplementedException();
        }
    }
}
