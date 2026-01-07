using WordRepeat.Core.Infrastructures;

namespace WordRepeat.Core.Models
{
    public class HistoryTypes
    {
        public Guid Id { get; set; } 
        public string NameType { get; set; } = string.Empty;

        public static ResultCreateModel<HistoryTypes> Create(Guid id, string nameType)
        {
            return ResultCreateModel<HistoryTypes>.Success(new(id, nameType));
        }
        private HistoryTypes(Guid id, string nameType) 
        {
            Id = id;
            NameType = nameType;
        }
    }
}
