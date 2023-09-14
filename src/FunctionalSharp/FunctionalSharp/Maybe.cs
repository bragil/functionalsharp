﻿namespace FunctionalSharp;

/// <summary>
/// Maybe Monad
/// </summary>
/// <typeparam name="TValue">Value type</typeparam>
public readonly struct Maybe<TValue>
{
    private readonly TValue value;
    public readonly bool HasValue => Utils.HasValue(value);

    internal Maybe(TValue value)
        => this.value = value;

    internal Maybe(None _) 
        => value = default;

    internal TValue GetValue() => value;

    public TValue GetValueOrElse(TValue fallback)
            => HasValue ? value : fallback;

    public T Match<T>(Func<TValue, T> some, Func<T> none)
        => HasValue ? some(value) : none();

    /// <summary>
    /// Provides execution chaining.
    /// </summary>
    /// <typeparam name="T">Return type</typeparam>
    /// <param name="function">Function to be applied to the value</param>
    /// <returns>Result with new value</returns>
    public Maybe<T> Then<T>(Func<TValue, T> function)
        => HasValue ? function(value) : default;

    /// <summary>
    /// Provides execution chaining.
    /// </summary>
    /// <typeparam name="T">Return type</typeparam>
    /// <param name="function">Function to be applied to the value</param>
    /// <returns>Result with new value</returns>
    public Maybe<T> Then<T>(Func<TValue, Maybe<T>> function)
            => HasValue ? function(value) : default;


    public static implicit operator Maybe<TValue>(TValue value)
            => new(value);

    public static implicit operator Maybe<TValue>(None none)
        => new(none);
}

public static class Maybe
{
    public static Maybe<T> Of<T>(T value) => new Maybe<T>(value);

    public static Maybe<T> Empty<T>()
        => new Maybe<T>(None.Create());
}