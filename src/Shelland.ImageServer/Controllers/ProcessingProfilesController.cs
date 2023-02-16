// Created on 31/05/2021 19:47 by Andrey Laserson

using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using Shelland.ImageServer.AppServices.Services.Abstract.Data;
using Shelland.ImageServer.Core.Other;
using Shelland.ImageServer.Models.Dto.Request;

namespace Shelland.ImageServer.Controllers;

[Route("processing-profiles")]
public class ProcessingProfilesController : BaseAppController
{
    private readonly IProcessingProfileDataService processingProfileDataService;

    public ProcessingProfilesController(IProcessingProfileDataService processingProfileDataService)
    {
        this.processingProfileDataService = processingProfileDataService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var profiles = await this.processingProfileDataService.GetProfiles();
        return Ok(profiles);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var profile = await this.processingProfileDataService.GetById(id);
        return OkOrNotFound(profile);
    }

    [HttpPost]
    [FeatureGate(Constants.FeatureFlags.ProcessingProfiles)]
    public async Task<IActionResult> Post([Required, FromForm] CreateProcessingProfileRequestDto model)
    {
        var profile = await this.processingProfileDataService.Create(model.Name, model.Parameters.Select(x => x.ToDomain()).ToList().AsReadOnly());
        return Ok(profile);
    }
}