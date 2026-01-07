namespace WordRepeat.DataAccess.Sqlite.Models
{
    public class WordsPairEntity
    {
        public Guid Id { get; set; }
        public string Word { get; set; } = string.Empty;
        public string Tranclate { get; set; } = string.Empty;
    }
}
