namespace FunctionalSharp;

internal static class Utils
{
    public static bool HasValue<T>(T value)
        => !EqualityComparer<T>.Default.Equals(value, default);

    public static bool HasError(Error error)
        => !EqualityComparer<Error>.Default.Equals(error, default);

    
    public static Result<T> TryCatch<T>(Func<T> function)
    {
        try
        {
            return function();
        }
        catch (Exception ex)
        {
            return new Error(ex.Message, ex);
        }
    }

    public static Result<T> TryCatch<T>(Func<Result<T>> function)
    {
        try
        {
            return function();
        }
        catch (Exception ex)
        {
            return new Error(ex.Message, ex);
        }
    }

    public static Result<Unit> TryCatch(Action function)
    {
        try
        {
            function();
            return Unit.Create();
        }
        catch (Exception ex)
        {
            return new Error(ex.Message, ex);
        }
    }
}
