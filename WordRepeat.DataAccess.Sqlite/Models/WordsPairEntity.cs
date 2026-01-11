namespace WordRepeat.DataAccess.Sqlite.Models
{
    public class WordsPairEntity
    {
        public Guid Id { get; set; }
        public string Word { get; set; } = string.Empty;
        public string Translate { get; set; } = string.Empty;
    }
}
