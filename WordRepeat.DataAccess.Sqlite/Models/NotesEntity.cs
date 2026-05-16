namespace WordRepeat.DataAccess.Sqlite.Models
{
    public class NotesEntity
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime DateUpdate { get; set; }
    }
}
