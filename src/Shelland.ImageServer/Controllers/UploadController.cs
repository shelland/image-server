// Created on 08/02/2021 15:59 by Andrey Laserson

#region Usings

using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using Shelland.ImageServer.AppServices.Services.Abstract.Common;
using Shelland.ImageServer.AppServices.Services.Abstract.Data;
using Shelland.ImageServer.AppServices.Services.Abstract.Storage;
using Shelland.ImageServer.Core.Models.Domain;
using Shelland.ImageServer.Core.Models.Other;
using Shelland.ImageServer.Core.Other;
using Shelland.ImageServer.Models.Dto.Request;
using Shelland.ImageServer.Models.Dto.Response;

#endregion

namespace Shelland.ImageServer.Controllers;

public class UploadController : BaseAppController
{
    private readonly IImageThumbnailService imageThumbnailService;
    private readonly IFileService fileService;
    private readonly IMapper mapper;
    private readonly IImageUploadDataService imageUploadDataService;

    public UploadController(
        IImageThumbnailService imageThumbnailService,
        IMapper mapper,
        IFileService fileService,
        IImageUploadDataService imageUploadDataService
    )
    {
        this.imageThumbnailService = imageThumbnailService;
        this.mapper = mapper;
        this.fileService = fileService;
        this.imageUploadDataService = imageUploadDataService;
    }

    [HttpPost]
    [FeatureGate(Constants.FeatureFlags.ImageResizing)]
    public async Task<IActionResult> Upload([Required, FromForm] ImageUploadParamsDto paramsDto, CancellationToken cancellationToken)
    {
        var file = this.GetDefaultFile();

        if (file == null)
        {
            return BadRequest();
        }

        var requestParamsModel = this.mapper.Map<ImageUploadParamsModel>(paramsDto);

        await using var imageStream = file.OpenReadStream();

        var job = new ImageUploadJob(
            Stream: imageStream,
            Params: requestParamsModel,
            IpAddress: IpAddress
        );

        var result = await this.imageThumbnailService.ProcessThumbnails(job, cancellationToken);
        var response = this.mapper.Map<ImageUploadResultDto>(result);

        return Ok(response);
    }

    [HttpGet("{id:Guid}")]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var upload = await this.imageUploadDataService.GetById(id);
        return OkOrNotFound(upload);
    }

    [HttpDelete("{id:Guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var upload = await this.imageUploadDataService.GetById(id);

        if (upload == null)
        {
            return NotFound();
        }

        var uploadModel = this.mapper.Map<ImageUploadModel>(upload);

        // Delete files from the disk
        this.fileService.Delete(uploadModel.GetAllFilePaths());

        // Remove a database record
        await this.imageUploadDataService.Delete(upload.Id);

        return Ok(id);
    }
}