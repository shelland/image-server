// Created on 03/03/2021 18:33 by Andrey Laserson

using System.Security.Cryptography;
using System.Text;

namespace Shelland.ImageServer.Core.Infrastructure.Extensions;

public static class StringExtensions
{
    /// <summary>
    /// Returns a hashed representation of the input string
    /// </summary>
    public static string GetMD5Hash(this string src)
    {
        var inputBytes = Encoding.ASCII.GetBytes(src);
        var hashBytes = MD5.HashData(inputBytes);

        var sb = new StringBuilder();

        foreach (var b in hashBytes)
        {
            sb.Append(b.ToString("X2"));
        }

        return sb.ToString();
    }
}