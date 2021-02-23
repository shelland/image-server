// Created on 15/02/2021 20:29 by Andrey Laserson

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shelland.ImageServer.AppServices.Services.Abstract.Processing;
using Shelland.ImageServer.Models.Dto.Response;

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
    }
}