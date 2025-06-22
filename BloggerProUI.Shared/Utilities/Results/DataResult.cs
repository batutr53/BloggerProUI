using BloggerProUI.Shared.Utilities.Results;

public class DataResult<T> : Result, IDataResult<T>
{
    public DataResult() { } // Parametresiz constructor şart
    public DataResult(T data, bool success, string[] message, int httpStatusCode, string statusCode, int count)
    : base(success, message, httpStatusCode, statusCode)
    {
        Data = data;
        Count = count;
    }

    public DataResult(T data, bool success, string message, int httpStatusCode, string statusCode)
        : base(success, message == null ? null : new[] { message }, httpStatusCode, statusCode)
    {
        Data = data;
    }

    public DataResult(T data, bool success, string[] message, int httpStatusCode, string statusCode)
        : base(success, message, httpStatusCode, statusCode)
    {
        Data = data;
    }

    public DataResult(T data, bool success) : base(success)
    {
        Data = data;
    }

    public DataResult(T data) : base(true)
    {
        Data = data;
    }

    public T Data { get; set; }  // !!! GET ve SET zorunlu
    public int Count { get; set; } // Aynı şekilde
}
