namespace WordRepeat.DataAccess.Sqlite.Models
{
    public class HistoryAddEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateOnly Date { get; set; }
        public int CountAdd { get; set; }
    }
}
