using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WordRepeat.DataAccess.Sqlite.Models;

namespace WordRepeat.DataAccess.Sqlite.Configurations
{
    public class HistoryTrainConfiguration : IEntityTypeConfiguration<HistoryTrainEntity>
    {
        public void Configure(EntityTypeBuilder<HistoryTrainEntity> builder)
        {
            throw new NotImplementedException();
        }
    }
}
