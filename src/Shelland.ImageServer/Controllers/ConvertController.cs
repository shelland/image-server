// Created on 15/02/2021 20:29 by Andrey Laserson

#region Usings

using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shelland.ImageServer.AppServices.Services.Abstract.Processing;
using Shelland.ImageServer.Core.Infrastructure.Extensions;
using Shelland.ImageServer.Core.Models.Enums;
using Shelland.ImageServer.Core.Models.Other;
using Shelland.ImageServer.Core.Other;
using Shelland.ImageServer.Models.Dto.Response;

#endregion

namespace Shelland.ImageServer.Controllers
{
    public class ConvertController : BaseAppController
    {
        private readonly IImageConvertingService imageConvertingService;

        public ConvertController(IImageConvertingService imageConvertingService)
        {
            this.imageConvertingService = imageConvertingService;
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

            return Json(new Response<string>(imgResult));
        }

        [HttpPost("format/{type}")]
        public async Task<IActionResult> ToJpeg(OutputImageFormat type, CancellationToken cancellationToken)
        {
            var file = this.GetDefaultFile();

            if (file == null)
            {
                return BadRequest();
            }

            return await File(file, type, cancellationToken);
        }
        
        private async Task<IActionResult> File(IFormFile file, OutputImageFormat format, CancellationToken cancellationToken)
        {
            var outputStream = new MemoryStream();
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
}