// Created on 15/02/2021 20:29 by Andrey Laserson

#region Usings

using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using Microsoft.IO;
using Shelland.ImageServer.AppServices.Services.Abstract.Processing;
using Shelland.ImageServer.Core.Infrastructure.Extensions;
using Shelland.ImageServer.Core.Models.Enums;
using Shelland.ImageServer.Core.Models.Other;
using Shelland.ImageServer.Core.Other;

#endregion

namespace Shelland.ImageServer.Controllers;

public class ConvertController : BaseAppController
{
    private readonly IImageConvertingService imageConvertingService;
    private readonly RecyclableMemoryStreamManager recyclableMemoryStreamManager;

    public ConvertController(IImageConvertingService imageConvertingService, RecyclableMemoryStreamManager recyclableMemoryStreamManager)
    {
        this.imageConvertingService = imageConvertingService;
        this.recyclableMemoryStreamManager = recyclableMemoryStreamManager;
    }

    [HttpPost("base64")]
    public async Task<IActionResult> ToBase64()
    {
        var file = this.GetDefaultFile();

        if (file == null)
        {
            return BadRequest();
        }

        await using var imageStream = file.OpenReadStream();
        var imgResult = await this.imageConvertingService.ImageToBase64(imageStream);

        return Ok(imgResult);
    }

    [HttpPost("format/{type}")]
    [FeatureGate(Constants.FeatureFlags.ImageConverting)]
    public async Task<IActionResult> ConvertImage([FromRoute] OutputImageFormat type, CancellationToken cancellationToken)
    {
        var file = this.GetDefaultFile();

        if (file == null)
        {
            return BadRequest();
        }

        return await ConvertFile(file, type, cancellationToken);
    }

    private async Task<IActionResult> ConvertFile(IFormFile file, OutputImageFormat format, CancellationToken cancellationToken)
    {
        var outputStream = new RecyclableMemoryStream(this.recyclableMemoryStreamManager);
        await using var imageStream = file.OpenReadStream();
            
        await this.imageConvertingService.ImageToFormat(imageStream, new StreamImageSavingParamsModel
        {
            Format = OutputImageFormat.Jpeg,
            Quality = Constants.DefaultJpegQuality,
            OutputStream = outputStream
        }, cancellationToken);

        return new FileStreamResult(outputStream, format.GetMimeType());
    }
}