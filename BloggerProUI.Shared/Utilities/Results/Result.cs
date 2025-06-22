using BloggerProUI.Shared.Utilities.Results;

public class Result : IResult
{
    public Result() { } // BOŞ CONSTRUCTOR şart

    public Result(bool success)
    {
        Success = success;
        HttpStatusCode = success ? 200 : 400;
        StatusCode = HttpStatusCode.ToString();
        Message = success ? new[] { "Success" } : new[] { "Operation Failed" };
    }

    public Result(bool success, string[] message, int httpStatusCode, string statusCode)
    {
        Success = success;
        Message = message;
        HttpStatusCode = httpStatusCode;
        StatusCode = statusCode;
    }

    public bool Success { get; set; }
    public string[] Message { get; set; }
    public int HttpStatusCode { get; set; }
    public string StatusCode { get; set; }
}
