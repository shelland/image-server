// Created on 31/05/2021 19:47 by Andrey Laserson

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Shelland.ImageServer.Core.Models.Domain;
using Shelland.ImageServer.DataAccess.Abstract.Repository;
using Shelland.ImageServer.Models.Dto.Response;

namespace Shelland.ImageServer.Controllers
{
    public class ProcessingProfileController : BaseAppController
    {
        private readonly IProcessingProfileRepository processingProfileRepository;
        private readonly IMapper mapper;

        public ProcessingProfileController(IProcessingProfileRepository processingProfileRepository, IMapper mapper)
        {
            this.processingProfileRepository = processingProfileRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var profiles = await this.processingProfileRepository.GetProfiles();
            var profileModels = this.mapper.Map<List<ProcessingProfileModel>>(profiles);

            return Ok(new Response<List<ProcessingProfileModel>>(profileModels));
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var profile = await this.processingProfileRepository.GetProfileById(id);

            if (profile == null)
            {
                return NotFound();
            }

            var profileModel = this.mapper.Map<ProcessingProfileModel>(profile);
            return Ok(new Response<ProcessingProfileModel>(profileModel));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ProcessingProfileModel model)
        {
            var profile = await this.processingProfileRepository.Create(model);
            var profileModel = this.mapper.Map<ProcessingProfileModel>(profile);

            return Ok(new Response<ProcessingProfileModel>(profileModel));
        }
    }
}