// Created on 14/02/2021 21:32 by Andrey Laserson

using System.Linq;
using System.Threading.Tasks;
using ImageMagick;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using Shelland.ImageServer.AppServices.Services.Abstract.Common;
using Shelland.ImageServer.Core.Other;

namespace Shelland.ImageServer.Controllers;

public class MetadataController : BaseAppController
{
    private readonly IImageReadingService imageReadingService;

    public MetadataController(IImageReadingService imageReadingService)
    {
        this.imageReadingService = imageReadingService;
    }

    [HttpPost]
    [FeatureGate(Constants.FeatureFlags.Metadata)]
    public async Task<IActionResult> Post()
    {
        var file = this.GetDefaultFile();

        if (file == null)
        {
            return BadRequest();
        }

        await using var imageStream = file.OpenReadStream();
        var image = await this.imageReadingService.Read(imageStream);

        var profile = image.GetExifProfile();

        if (profile == null)
        {
            return Ok(null);
        }

        var values = profile.Values.Where(x => x.DataType != ExifDataType.Undefined || x.DataType != ExifDataType.Unknown).ToList().AsReadOnly();
        return Ok(values);
    }
}