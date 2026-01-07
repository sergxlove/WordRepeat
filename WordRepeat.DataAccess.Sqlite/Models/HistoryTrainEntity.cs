namespace WordRepeat.DataAccess.Sqlite.Models
{
    public class HistoryTrainEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Result { get; set; }
        public int Total { get; set; }
        public DateOnly Date { get; set; }
    }
}
