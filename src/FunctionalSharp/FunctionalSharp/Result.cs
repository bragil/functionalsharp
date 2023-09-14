﻿namespace FunctionalSharp;

public readonly struct Result<TValue>
{
    private readonly Maybe<TValue> value;
    private readonly Error error;
    public readonly bool HasValue => value.HasValue;
    public readonly bool HasError => Utils.HasError(error);

    internal Result(TValue value)
    {
        this.value = value;
        error = default;
    }

    internal Result(Error error)
    {
        value = Maybe.Empty<TValue>();
        this.error = error;
    }

    internal Result(None _)
    {
        value = Maybe.Empty<TValue>();
        error = default;
    }

    internal TValue GetValue()
        => value.GetValue();

    internal Error GetError()
        => error;

    public Result<TValue> OnError(Action<Error> errorFunction)
    {
        if (HasError)
            errorFunction(error);

        return this;
    }

    public Result<TValue> OnSomeValue(Action<TValue> valueFunction)
    {
        if (HasValue)
            valueFunction(value.GetValue());

        return this;
    }

    public Result<TValue> OnNoneValue(Action noValueFunction)
    {
        if (!HasValue)
            noValueFunction();

        return this;
    }

    /// <summary>
    /// Return the value or the fallback (if not value).
    /// </summary>
    /// <param name="fallback">Value to be returned if no value</param>
    /// <returns>Value or fallback</returns>
    public TValue GetValueOrElse(TValue fallback)
        => HasError ? fallback : value.GetValueOrElse(fallback);

    /// <summary>
    /// Result pattern match
    /// </summary>
    /// <typeparam name="T">Return type</typeparam>
    /// <param name="some">Executed when contains value</param>
    /// <param name="none">Executed when not contains value</param>
    /// <param name="error">Executed when has error</param>
    /// <returns>Result of T</returns>
    public T Match<T>(Func<TValue, T> some, Func<T> none, Func<Error, T> error)
        => HasError ? error(this.error) : value.Match(some, none);


    public Result<T> Then<T>(Func<TValue, T> function)
        => (HasError, HasValue) switch
        {
            (true, _) => error,
            (false, true) => Try(function),
            (false, false) => None.Create()
        };

    public Result<T> Then<T>(Func<TValue, Result<T>> function)
        => (HasError, HasValue) switch
        {
            (true, _) => error,
            (false, true) => Try(function),
            (false, false) => None.Create()
        };

    public Result<Unit> Then(Action<TValue> function)
        => (HasError, HasValue) switch
        {
            (true, _) => error,
            (false, true) => Try(function),
            (false, false) => None.Create()
        };

    public async Task<Result<T>> Then<T>(Func<TValue, Task<T>> function)
        => (HasError, HasValue) switch
        {
            (true, _) => error,
            (false, true) => await TryAsync(function),
            (false, false) => None.Create()
        };

    public async Task<Result<T>> Then<T>(Func<TValue, Task<Result<T>>> function)
        => (HasError, HasValue) switch
        {
            (true, _) => error,
            (false, true) => await TryAsync(function),
            (false, false) => None.Create()
        };

    public async Task<Result<Unit>> Then(Func<TValue, Task> function)
        => (HasError, HasValue) switch
        {
            (true, _) => error,
            (false, true) => await TryAsync(function),
            (false, false) => None.Create()
        };

    private Result<Unit> Try(Action<TValue> function)
    {
        try
        {
            function(value.GetValueOrElse(default));
            return new Result<Unit>(Unit.Create());
        }
        catch (Exception ex)
        {
            return new Error(ex.Message, ex);
        }
    }

    private Result<T> Try<T>(Func<TValue, T> function)
    {
        try
        {
            return function(value.GetValueOrElse(default));
        }
        catch (Exception ex)
        {
            return new Error(ex.Message, ex);
        }
    }

    private Result<T> Try<T>(Func<TValue, Result<T>> function)
    {
        try
        {
            return function(value.GetValueOrElse(default));
        }
        catch (Exception ex)
        {
            return new Error(ex.Message, ex);
        }
    }

    private async Task<Result<Unit>> TryAsync(Func<TValue, Task> function)
    {
        try
        {
            await function(value.GetValueOrElse(default));
            return Unit.Create();
        }
        catch (Exception ex)
        {
            return new Error(ex.Message, ex);
        }
    }

    private async Task<Result<T>> TryAsync<T>(Func<TValue, Task<T>> function)
    {
        try
        {
            return await function(value.GetValueOrElse(default));
        }
        catch (Exception ex)
        {
            return new Error(ex.Message, ex);
        }
    }

    private async Task<Result<T>> TryAsync<T>(Func<TValue, Task<Result<T>>> function)
    {
        try
        {
            return await function(value.GetValueOrElse(default));
        }
        catch (Exception ex)
        {
            return new Error(ex.Message, ex);
        }
    }


    /// <summary>
    /// Implicit cast operator for return with value.
    /// </summary>
    /// <param name="theValue">Value</param>
    public static implicit operator Result<TValue>(TValue theValue)
        => new Result<TValue>(theValue);

    /// <summary>
    /// Implicit cast operator for error.
    /// </summary>
    /// <param name="error">Error</param>
    public static implicit operator Result<TValue>(Error error)
        => new Result<TValue>(error);

    /// <summary>
    /// Operador de cast implícito para None (nenhum valor).
    /// </summary>
    /// <param name="none">Objeto None</param>
    public static implicit operator Result<TValue>(None none)
        => new Result<TValue>(none);

    /// <summary>
    /// Operador de cast implícito para Opt[TValue].
    /// </summary>
    /// <param name="maybe">Objeto Opt[TValue]</param>
    public static implicit operator Result<TValue>(Maybe<TValue> maybe)
        => maybe.HasValue 
            ? maybe.GetValueOrElse(default) 
            : new Result<TValue>(None.Create());
}

public static class Result
{
    public static Result<T> Of<T>(T value)
        => value;
}