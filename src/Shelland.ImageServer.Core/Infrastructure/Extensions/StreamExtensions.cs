﻿// Created on 10/02/2021 12:17 by Andrey Laserson

using System.IO;
using System.Threading.Tasks;

namespace Shelland.ImageServer.Core.Infrastructure.Extensions
{
    public static class StreamExtensions
    {
        public static async Task<byte[]> ToByteArray(this Stream stream)
        {
            await using var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);

            var buffer = memoryStream.ToArray();

            return buffer;
        }

        public static void Reset(this Stream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);
        }
    }
}