// Created on 08/02/2021 18:26 by Andrey Laserson

using System;
using System.IO;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Microsoft.Extensions.Logging;
using Shelland.ImageServer.AppServices.Services.Abstract.Processing;
using Shelland.ImageServer.Core.Infrastructure.Exceptions;
using Shelland.ImageServer.Core.Infrastructure.Extensions;
using Shelland.ImageServer.Core.Models.Enums;
using Shelland.ImageServer.Core.Models.Other;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace Shelland.ImageServer.AppServices.Services.Processing
{
    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public class ImageProcessingService : IImageProcessingService
    {
        private readonly ILogger<ImageProcessingService> logger;

        public ImageProcessingService(ILogger<ImageProcessingService> logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public Image Process(ImageProcessingJob job)
        {
            try
            {
                // Check if both sizes are valid.
                Guard.Against.PositiveCondition(job.ThumbnailParams.Width == null && job.ThumbnailParams.Height == null);
                
                // If any parameter of Resize function == 0 then another size will be used in respect with aspect ratio
                var image = job.Image.Clone(x => x.Resize(job.ThumbnailParams.Width ?? 0, job.ThumbnailParams.Height ?? 0));
               
                // Check if any effect was requested and apply it if so
                if (job.ThumbnailParams.Effect.HasValue)
                {
                    this.ApplyEffect(image, job.ThumbnailParams.Effect.Value);
                }

                // Remove image metadata if such setting is set
                if (!job.Settings.KeepMetadata ?? true)
                {
                    image.Metadata.ExifProfile = null;
                }

                return image;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, ex.Message);
                throw new AppFlowException(AppFlowExceptionType.InvalidImageFormat);
            }
        }
        
        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public async Task<Image> Load(Stream stream)
        {
            return await Image.LoadAsync(stream);
        }

        private void ApplyEffect(Image image, ThumbnailEffectType effect)
        {
            switch (effect)
            {
                case ThumbnailEffectType.Grayscale:
                    image.Mutate(x => x.Grayscale());
                    break;

                case ThumbnailEffectType.Sepia:
                    image.Mutate(x => x.Sepia());
                    break;
            }
        }
    }
}