namespace FunctionalSharp.Tasks;

public static class TaskExtensions
{
    /// <summary>
    /// <![CDATA[
    /// Chaining a task of Result<T>.
    /// ]]>
    /// </summary>
    /// <typeparam name="TIn">Value type</typeparam>
    /// <param name="source">Source task</param>
    /// <param name="function">Function to be applied to the task</param>
    /// <returns><![CDATA[New task of Result<T>]]></returns>
    public static async Task<Result<TOut>> Then<TIn, TOut>(this Task<Result<TIn>> source,
                                                Func<TIn, Task<Result<TOut>>> function)
    {
        try
        {
            var res = await source;
            return res.HasError ? res.GetError() : await function(res.GetValueOrElse(default));
        }
        catch (Exception ex)
        {
            return new Error(ex.Message, ex);
        }
    }

    /// <summary>
    /// Fire and forget a task.
    /// </summary>
    /// <param name="task">Task</param>
    /// <param name="errorHandler">Error handler</param>
    public static void FireAndForget(this Task task, Action<Exception> errorHandler = null)
    {
        task.ContinueWith(t =>
        {
            if (t.IsFaulted && errorHandler != null)
                errorHandler(t.Exception);
        }, TaskContinuationOptions.OnlyOnFaulted);
    }

    /// <summary>
    /// Retrying a task {maxRetries} times, with delay.
    /// </summary>
    /// <typeparam name="TResult">Value type</typeparam>
    /// <param name="taskFactory">Function to be executed</param>
    /// <param name="maxRetries">Max of retries</param>
    /// <param name="delay">Delay between retries</param>
    /// <returns><![CDATA[Task<TResult>]]></returns>
    public static async Task<TResult> Retry<TResult>(
                                        this Func<Task<TResult>> taskFactory,
                                        int maxRetries,
                                        TimeSpan delay)
    {
        for (int i = 0; i < maxRetries; i++)
        {
            try
            {
                return await taskFactory().ConfigureAwait(false);
            }
            catch
            {
                if (i == maxRetries - 1)
                    throw;
                await Task.Delay(delay).ConfigureAwait(false);
            }
        }

        return default; // Should not be reached
    }

    /// <summary>
    /// If the task takes longer than {timeout}, an exception is thrown.
    /// </summary>
    /// <param name="task">The task</param>
    /// <param name="timeout">Timeout</param>
    /// <returns>Task</returns>
    /// <exception cref="TimeoutException"></exception>
    public static async Task WithTimeout(this Task task, TimeSpan timeout)
    {
        var delayTask = Task.Delay(timeout);
        var completedTask = await Task.WhenAny(task, delayTask).ConfigureAwait(false);
        if (completedTask == delayTask)
            throw new TimeoutException();

        await task;
    }
}
