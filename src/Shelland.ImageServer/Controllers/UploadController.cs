// Created on 08/02/2021 15:59 by Andrey Laserson

using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Shelland.ImageServer.AppServices.Services.Abstract.Common;
using Shelland.ImageServer.Core.Models.Other;
using Shelland.ImageServer.DataAccess.Abstract.Repository;
using Shelland.ImageServer.Models.Dto.Request;
using Shelland.ImageServer.Models.Dto.Response;

namespace Shelland.ImageServer.Controllers
{
    [Route("upload")]
    public class UploadController : BaseAppController
    {
        private readonly IImageUploadService imageUploadService;
        private readonly IImageUploadRepository imageUploadRepository;

        private readonly IMapper mapper;

        public UploadController(
            IImageUploadService imageUploadService, 
            IImageUploadRepository imageUploadRepository, 
            IMapper mapper)
        {
            this.imageUploadService = imageUploadService;
            this.imageUploadRepository = imageUploadRepository;
            this.mapper = mapper;
        }
        
        [HttpPost]
        public async Task<IActionResult> Upload([Required, FromForm] ImageUploadParamsDto paramsDto)
        {
            var file = Request.Form.Files.FirstOrDefault();

            if (file == null)
            {
                return BadRequest();
            }

            var userIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var thumbnailInputModel = this.mapper.Map<ImageUploadParamsModel>(paramsDto);

            var job = new ImageUploadJob
            {
                IpAddress = userIpAddress,
                Params = thumbnailInputModel,
                Stream = file.OpenReadStream()
            };

            var response = await this.imageUploadService.Process(job);

            return Ok(this.mapper.Map<ImageUploadResultDto>(response));
        }

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            var upload = await this.imageUploadRepository.GetById(id);

            if (upload == null)
            {
                return NotFound();
            }

            return Ok(upload);
        }

        [HttpDelete("{id:Guid}")]
        public async Task<ImageUploadResultDto> Delete([FromRoute] Guid id)
        {
            return null;
        }
    }
}