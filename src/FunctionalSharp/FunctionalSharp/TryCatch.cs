namespace FunctionalSharp;

public static class TryCatch
{
    /// <summary>
    /// Functional replace for try..catch block.
    /// </summary>
    /// <param name="function">Function to execute</param>
    /// <param name="errorHandler">Error handler (optional)</param>
    /// <returns><![CDATA[Result<Unit>]]></returns>
    public static Result<Unit> Try(Action function, Action<Error> errorHandler = null)
    {
        try
        {
            function();
            return Unit.Create();
        }
        catch (Exception ex)
        {
            var error = new Error(ex.Message, ex);
            errorHandler?.Invoke(error);

            return error;
        }
    }

    /// <summary>
    /// Functional replace for try..catch block.
    /// </summary>
    /// <typeparam name="T">Value type</typeparam>
    /// <param name="function">Function to execute</param>
    /// <param name="errorHandler">Error handler (optional)</param>
    /// <returns><![CDATA[Result<T>]]></returns>
    public static Result<T> Try<T>(Func<T> function, Action<Error> errorHandler = null)
    {
        try
        {
            return function();
        }
        catch (Exception ex)
        {
            var error = new Error(ex.Message, ex);
            errorHandler?.Invoke(error);

            return error;
        }
    }


    /// <summary>
    /// Functional replace for try..catch block.
    /// </summary>
    /// <typeparam name="T">Value type</typeparam>
    /// <param name="function">Function to execute</param>
    /// <param name="errorHandler">Error handler (optional)</param>
    /// <returns><![CDATA[Result<T>]]></returns>
    public static Result<T> Try<T>(Func<Result<T>> function, Action<Error> errorHandler = null)
    {
        try
        {
            return function();
        }
        catch (Exception ex)
        {
            var error = new Error(ex.Message, ex);
            errorHandler?.Invoke(error);

            return error;
        }
    }
}
