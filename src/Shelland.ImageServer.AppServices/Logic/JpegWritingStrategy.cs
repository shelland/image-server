// Created on 16/03/2021 21:56 by Andrey Laserson

using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ImageMagick;
using ImageMagick.Formats;
using Shelland.ImageServer.AppServices.Services.Abstract.Common;

namespace Shelland.ImageServer.AppServices.Logic;

/// <summary>
/// JPEG writing strategy. Includes some specific JPEG-related features
/// </summary>
public class JpegWritingStrategy : IImageWritingStrategy
{
    private readonly uint quality;

    public JpegWritingStrategy(uint quality)
    {
        this.quality = quality;
    }

    public async Task Write(MagickImage image, Stream outputStream, CancellationToken cancellationToken)
    {
        image.Quality = this.quality;
        image.Settings.Interlace = Interlace.Jpeg;
        image.AdaptiveBlur(0.05);
            
        await image.WriteAsync(outputStream, new JpegWriteDefines
        {
            SamplingFactor = JpegSamplingFactor.Ratio420,
            DctMethod = JpegDctMethod.Float
        }, cancellationToken);
    }
}