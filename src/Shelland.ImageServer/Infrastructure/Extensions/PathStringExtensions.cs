﻿// Created on 14/02/2021 16:54 by Andrey Laserson

using System;
using Microsoft.AspNetCore.Http;

namespace Shelland.ImageServer.Infrastructure.Extensions;

public static class PathStringExtensions
{
    public static bool StartsWithNormalizedSegments(this PathString path, PathString other)
    {
        if (other.HasValue && other.Value?.EndsWith('/') == true)
        {
            return path.StartsWithSegments(other.Value[..^1]);
        }

        return path.StartsWithSegments(other);
    }

    public static bool StartsWithNormalizedSegments(this PathString path, PathString other, StringComparison comparisonType)
    {
        if (other.HasValue && other.Value?.EndsWith('/') == true)
        {
            return path.StartsWithSegments(other.Value[..^1], comparisonType);
        }

        return path.StartsWithSegments(other, comparisonType);
    }

    public static bool StartsWithNormalizedSegments(this PathString path, PathString other, out PathString remaining)
    {
        if (other.HasValue && other.Value?.EndsWith('/') == true)
        {
            return path.StartsWithSegments(other.Value[..^1], out remaining);
        }

        return path.StartsWithSegments(other, out remaining);
    }

    public static bool StartsWithNormalizedSegments(this PathString path, PathString other, StringComparison comparisonType, out PathString remaining)
    {
        if (other.HasValue && other.Value?.EndsWith('/') == true)
        {
            return path.StartsWithSegments(other.Value[..^1], comparisonType, out remaining);
        }

        return path.StartsWithSegments(other, comparisonType, out remaining);
    }
}