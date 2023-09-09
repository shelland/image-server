// Created on 16/03/2021 21:22 by Andrey Laserson

using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NetVips;
using Shelland.ImageServer.AppServices.Services.Abstract.Common;

namespace Shelland.ImageServer.AppServices.Logic;

/// <summary>
/// Image writing context that depends on selected strategy
/// </summary>
public class ImageWritingContext
{
    private readonly IImageWritingStrategy writingStrategy;

    public ImageWritingContext(IImageWritingStrategy writingStrategy)
    {
        this.writingStrategy = writingStrategy;
    }

    public async Task Write(Image image, Stream outputStream, CancellationToken cancellationToken)
    {
        await this.writingStrategy.Write(image, outputStream, cancellationToken);
    }
}