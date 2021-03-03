﻿// Created on 14/02/2021 21:32 by Andrey Laserson

using System.Linq;
using System.Threading.Tasks;
using ImageMagick;
using Microsoft.AspNetCore.Mvc;
using Shelland.ImageServer.AppServices.Services.Abstract.Common;

namespace Shelland.ImageServer.Controllers
{
    public class MetadataController : BaseAppController
    {
        private readonly IImageLoadingService imageLoadingService;

        public MetadataController(IImageLoadingService imageLoadingService)
        {
            this.imageLoadingService = imageLoadingService;
        }

        [HttpPost]
        public async Task<IActionResult> Post()
        {
            var file = this.GetDefaultFile();

            if (file == null)
            {
                return BadRequest();
            }

            await using var imageStream = file.OpenReadStream();
            var image = await this.imageLoadingService.Load(imageStream);

            var profile = image.GetExifProfile();

            if (profile == null)
            {
                return Ok(null);
            }

            var values = profile.Values.Where(x => x.DataType != ExifDataType.Undefined || x.DataType != ExifDataType.Unknown);

            return Ok(values);
        }
    }
}