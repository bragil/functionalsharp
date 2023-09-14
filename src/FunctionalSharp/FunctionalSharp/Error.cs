namespace FunctionalSharp;

public interface IError
{
    string Message { get; }
}

public struct Error: IError
{
    public string Message { get; }

    public Exception Exception { get; }

    public object ErrorData { get; }

    public Error(string message, Exception ex = null, object errorData = null)
    {
        Message = message;
        Exception = ex;
        ErrorData = errorData;
    }
}