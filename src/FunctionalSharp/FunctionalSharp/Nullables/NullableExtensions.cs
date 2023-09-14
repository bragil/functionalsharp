namespace FunctionalSharp.Nullables;

public static class NullableExtensions
{
    public static Maybe<T> ToMaybe<T>(this T? nullable) where T : struct
        => nullable.HasValue ? nullable.Value : default;

    public static Result<T> ToResult<T>(this T? nullable) where T : struct
        => nullable ?? default;

    public static TOut? Then<TIn, TOut>(this TIn? nullable, Func<TIn, TOut> function)
        where TIn : struct
        where TOut : struct
        => nullable.HasValue ? function(nullable.Value) : default;

    public static TOut? Then<TIn, TOut>(this TIn? nullable, Func<TIn, TOut?> function)
        where TIn : struct
        where TOut : struct
        => nullable.HasValue ? function(nullable.Value) : default;

    public static TOut Match<TIn, TOut>(this TIn? nullable,
            Func<TIn, TOut> some,
            Func<None, TOut> none = null)
            where TIn : struct
            where TOut : class
        => nullable.HasValue ? some(nullable.Value) : none(None.Create());
}
