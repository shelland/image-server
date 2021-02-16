// Created on 14/02/2021 21:32 by Andrey Laserson

using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shelland.ImageServer.AppServices.Services.Abstract.Processing;
using SixLabors.ImageSharp.Metadata.Profiles.Exif;

namespace Shelland.ImageServer.Controllers
{
    [Route("metadata")]
    public class MetadataController : BaseAppController
    {
        private readonly IImageProcessingService imageProcessingService;

        public MetadataController(IImageProcessingService imageProcessingService)
        {
            this.imageProcessingService = imageProcessingService;
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
            var image = await this.imageProcessingService.Load(imageStream);

            var profile = image.Metadata.ExifProfile;

            if (profile == null)
            {
                return Ok(null);
            }

            var values = profile.Values.Where(x => x.DataType != ExifDataType.Undefined || x.DataType != ExifDataType.Unknown);

            return Ok(values);
        }
    }
}