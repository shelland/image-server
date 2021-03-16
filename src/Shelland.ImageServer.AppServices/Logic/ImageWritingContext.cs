// Created on 16/03/2021 21:22 by Andrey Laserson

using System.IO;
using System.Threading.Tasks;
using ImageMagick;
using Shelland.ImageServer.AppServices.Services.Abstract.Common;

namespace Shelland.ImageServer.AppServices.Logic
{
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

        public async Task Write(MagickImage image, Stream outputStream)
        {
            await this.writingStrategy.Write(image, outputStream);
        }
    }
}