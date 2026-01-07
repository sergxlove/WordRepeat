namespace WordRepeat.Core.Infrastructures
{
    public class ResultCreateModel<T>
    {
        public T Value { get; }
        public string Error { get; }
        public bool IsSuccess
        {
            get
            {
                if (Error == string.Empty) return true;
                return false;
            }
        }

        private ResultCreateModel(T value, string error)
        {
            Value = value;
            Error = error;
        }

        public static ResultCreateModel<T> Success(T value)
        {
            return new ResultCreateModel<T>(value, string.Empty);
        }

        public static ResultCreateModel<T> Failure(string error)
        {
            return new ResultCreateModel<T>(default!, error);
        }
    }
}
