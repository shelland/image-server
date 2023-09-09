// Created on 16/03/2021 21:55 by Andrey Laserson

using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NetVips;

namespace Shelland.ImageServer.AppServices.Services.Abstract.Common;

/// <summary>
/// Defines a format dependent image writing strategy
/// </summary>
public interface IImageWritingStrategy
{
    Task Write(Image image, Stream outputStream, CancellationToken cancellationToken);
}