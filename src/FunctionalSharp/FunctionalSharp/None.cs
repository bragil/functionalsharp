﻿namespace FunctionalSharp;

/// <summary>
/// Safety replacement for null.
/// </summary>
public struct None
{
    public static None Create() => new None();
}
