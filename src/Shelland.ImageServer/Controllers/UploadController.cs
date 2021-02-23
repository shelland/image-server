// Created on 08/02/2021 15:59 by Andrey Laserson

using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Shelland.ImageServer.AppServices.Services.Abstract.Common;
using Shelland.ImageServer.AppServices.Services.Abstract.Storage;
using Shelland.ImageServer.Core.Models.Data;
using Shelland.ImageServer.Core.Models.Domain;
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
        private readonly IFileService fileService;

        private readonly IMapper mapper;

        public UploadController(
            IImageUploadService imageUploadService,
            IImageUploadRepository imageUploadRepository,
            IMapper mapper,
            IFileService fileService)
        {
            this.imageUploadService = imageUploadService;
            this.imageUploadRepository = imageUploadRepository;
            this.mapper = mapper;
            this.fileService = fileService;
        }

        [HttpPost]
        public async Task<IActionResult> Upload([Required, FromForm] ImageUploadParamsDto paramsDto)
        {
            var file = this.GetDefaultFile();

            if (file == null)
            {
                return BadRequest();
            }

            var userIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var thumbnailInputModel = this.mapper.Map<ImageUploadParamsModel>(paramsDto);

            await using var imageStream = file.OpenReadStream();

            var job = new ImageUploadJob
            {
                IpAddress = userIpAddress,
                Params = thumbnailInputModel,
                Stream = imageStream,
                ExpirationDate = thumbnailInputModel.Lifetime.HasValue ? 
                    DateTimeOffset.UtcNow.AddSeconds(thumbnailInputModel.Lifetime.Value) : 
                    null
            };

            var response = await this.imageUploadService.RunProcessingJob(job);

            return Ok(new Response<ImageUploadResultModel>(response));
        }

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            var upload = await this.imageUploadRepository.GetById(id);

            if (upload == null)
            {
                return NotFound();
            }

            return Ok(new Response<ImageUploadDbModel>(upload));
        }

        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var upload = await this.imageUploadRepository.GetById(id);

            if (upload == null)
            {
                return NotFound();
            }

            // Delete files from the disk
            this.fileService.Delete(upload.GetAllFilePaths());

            // Remove a database record
            await this.imageUploadRepository.Delete(upload.Id);

            return Ok(new Response<Guid>(id));
        }
    }
}