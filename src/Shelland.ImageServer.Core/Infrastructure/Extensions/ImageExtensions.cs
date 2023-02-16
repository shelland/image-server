// Created on 10/02/2021 12:38 by Andrey Laserson

using System.Linq;
using System.Text;

namespace Shelland.ImageServer.Core.Infrastructure.Extensions;

public static class ImageExtensions
{
    /// <summary>
    /// Checks file header to verify an image format
    /// </summary>
    public static bool IsValidImage(this byte[] imageBytes)
    {
        var headers = new[]
        {
            Encoding.ASCII.GetBytes("BM"), // BMP
            Encoding.ASCII.GetBytes("GIF"), // GIF

            new byte[]
            {
                137, 80, 78, 71 // PNG
            },

            new byte[]
            {
                73, 73, 42 // TIFF
            },

            new byte[]
            {
                77, 77, 42 // TIFF
            },

            new byte[]
            {
                255, 216, 255, 224 // JPEG
            },

            new byte[]
            {
                255, 216, 255, 225 // JPEG CANON
            }
        };
        
        return headers.Any(x => x.SequenceEqual(imageBytes.Take(x.Length)));
    }
}