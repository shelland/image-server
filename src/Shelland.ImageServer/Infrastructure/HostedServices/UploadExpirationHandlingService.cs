// Created on 23/02/2021 11:28 by Andrey Laserson

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Shelland.ImageServer.AppServices.Services.Abstract.Storage;
using Shelland.ImageServer.Core.Models.Domain;
using Shelland.ImageServer.Core.Other;
using Shelland.ImageServer.DataAccess.Abstract.Repository;

namespace Shelland.ImageServer.Infrastructure.HostedServices
{
    /// <summary>
    /// Background service to check and remove expired image uploads
    /// </summary>
    public class UploadExpirationHandlingService : BackgroundService
    {
        private readonly ILogger<UploadExpirationHandlingService> logger;
        private readonly IImageUploadRepository imageUploadRepository;
        private readonly IFileService fileService;
        private readonly IMapper mapper;

        public UploadExpirationHandlingService(
            ILogger<UploadExpirationHandlingService> logger,
            IImageUploadRepository imageUploadRepository,
            IFileService fileService,
            IMapper mapper)
        {
            this.logger = logger;
            this.imageUploadRepository = imageUploadRepository;
            this.fileService = fileService;
            this.mapper = mapper;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                this.logger.LogInformation($"Running a job");

                // Fetch all uploads that are expired at this time
                var expiredUploads = await this.imageUploadRepository.GetExpiredUploads();
                var expiredUploadModels = this.mapper.Map<List<ImageUploadModel>>(expiredUploads);

                if (expiredUploadModels.Any())
                {
                    this.logger.LogInformation($"Found {expiredUploads.Count} expired uploads");

                    foreach (var upload in expiredUploadModels)
                    {
                        await this.Delete(upload);
                    }
                }
                else
                {
                    this.logger.LogInformation($"No expired uploads to remove");
                }

                await Task.Delay(TimeSpan.FromSeconds(Constants.ExpiredUploadsServiceRunInterval), stoppingToken);
            }
        }

        private async Task Delete(ImageUploadModel uploadDb)
        {
            try
            {
                this.logger.LogInformation($"Deleting a record with id {uploadDb.Id} and expiration date {uploadDb.ExpiresAt.Value}");

                // Delete files
                this.fileService.Delete(uploadDb.GetAllFilePaths());

                // Delete a DB record
                await this.imageUploadRepository.Delete(uploadDb.Id);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, ex.Message);
            }
        }
    }
}