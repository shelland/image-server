// Created on 08/02/2021 15:59 by Andrey Laserson

#region Usings

using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Shelland.ImageServer.AppServices.Services.Abstract.Common;
using Shelland.ImageServer.AppServices.Services.Abstract.Storage;
using Shelland.ImageServer.Core.Models.Domain;
using Shelland.ImageServer.Core.Models.Other;
using Shelland.ImageServer.DataAccess.Abstract.Repository;
using Shelland.ImageServer.Models.Dto.Request;
using Shelland.ImageServer.Models.Dto.Response;

#endregion

namespace Shelland.ImageServer.Controllers
{
    public class UploadController : BaseAppController
    {
        private readonly IImageThumbnailService imageThumbnailService;
        private readonly IImageUploadRepository imageUploadRepository;
        private readonly IFileService fileService;

        private readonly IMapper mapper;

        public UploadController(
            IImageThumbnailService imageThumbnailService,
            IImageUploadRepository imageUploadRepository,
            IMapper mapper,
            IFileService fileService)
        {
            this.imageThumbnailService = imageThumbnailService;
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

            var requestParamsModel = this.mapper.Map<ImageUploadParamsModel>(paramsDto);

            await using var imageStream = file.OpenReadStream();

            var job = new ImageUploadJob
            {
                IpAddress = IpAddress,
                Params = requestParamsModel,
                Stream = imageStream
            };

            var response = await this.imageThumbnailService.ProcessThumbnails(job);

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

            var uploadModel = this.mapper.Map<ImageUploadModel>(upload);

            return Ok(new Response<ImageUploadModel>(uploadModel));
        }

        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var upload = await this.imageUploadRepository.GetById(id);
            
            if (upload == null)
            {
                return NotFound();
            }

            var uploadModel = this.mapper.Map<ImageUploadModel>(upload);

            // Delete files from the disk
            this.fileService.Delete(uploadModel.GetAllFilePaths());

            // Remove a database record
            await this.imageUploadRepository.Delete(upload.Id);

            return Ok(new Response<Guid>(id));
        }
    }
}