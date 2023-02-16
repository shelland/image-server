// Created on 03/02/2023 16:47 by shell

using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using Shelland.ImageServer.Core.Other;
using Shelland.ImageServer.FaceDetection.Services.Abstract;
using Shelland.ImageServer.Models.Dto.Request;

namespace Shelland.ImageServer.Controllers;

[Route("face-detection")]
public class FaceDetectionController : BaseAppController
{
    private readonly IFaceDetectionService faceDetectionService;

    public FaceDetectionController(IFaceDetectionService faceDetectionService)
    {
        this.faceDetectionService = faceDetectionService;
    }

    [HttpPost]
    [FeatureGate(Constants.FeatureFlags.FaceRecognition)]
    public async Task<IActionResult> GetFaces([FromForm] DetectFacesParamsRequestDto? request, CancellationToken cancellationToken)
    {
        var file = this.GetDefaultFile();

        if (file == null)
        {
            return BadRequest();
        }

        await using var imageStream = file.OpenReadStream();
        var faces = await this.faceDetectionService.GetFaces(imageStream, request?.SaveDetectedFaces ?? false, cancellationToken);

        return Ok(faces);
    }
}