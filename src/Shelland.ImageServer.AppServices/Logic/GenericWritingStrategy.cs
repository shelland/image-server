// Created on 16/03/2021 21:57 by Andrey Laserson

using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NetVips;
using Shelland.ImageServer.AppServices.Services.Abstract.Common;

namespace Shelland.ImageServer.AppServices.Logic;

/// <summary>
/// Generic writing strategy for GIF and PNG files
/// </summary>
public class GenericWritingStrategy : IImageWritingStrategy
{
    public async Task Write(Image image, Stream outputStream, CancellationToken cancellationToken)
    {
        // await image.WriteAsync(outputStream, cancellationToken);
        await Task.Run(() => image.WriteToStream(outputStream, "jpeg"), cancellationToken);
    }
}