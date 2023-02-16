// Created on 05/02/2023 19:18 by shell

using System.Runtime.CompilerServices;
using System;

namespace Shelland.ImageServer.Core.Infrastructure.Extensions;

public static class ObjectExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [return: System.Diagnostics.CodeAnalysis.NotNull]
    public static T NotNull<T>(this T? obj, [CallerArgumentExpression(nameof(obj))] string msg = "") => obj != null ? 
        obj : throw new NullReferenceException(msg);
}