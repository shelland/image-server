// Created on 23/02/2021 11:28 by Andrey Laserson

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Shelland.ImageServer.AppServices.Services.Abstract.Data;
using Shelland.ImageServer.AppServices.Services.Abstract.Storage;
using Shelland.ImageServer.Core.Models.Domain;
using Shelland.ImageServer.Core.Other;

namespace Shelland.ImageServer.Infrastructure.HostedServices;

/// <summary>
/// Background service to check and remove expired image uploads
/// </summary>
public class UploadExpirationHandlingService : BackgroundService
{
    private readonly ILogger<UploadExpirationHandlingService> logger;
    private readonly IFileService fileService;
    private readonly IImageUploadDataService imageUploadDataService;

    public UploadExpirationHandlingService(
        ILogger<UploadExpirationHandlingService> logger,
        IFileService fileService,
        IImageUploadDataService imageUploadDataService
    )
    {
        this.logger = logger;
        this.fileService = fileService;
        this.imageUploadDataService = imageUploadDataService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(TimeSpan.FromSeconds(Constants.ExpiredUploadsServiceRunInterval));

        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            this.logger.LogInformation("Running an image expiration job");

            // Fetch all uploads that are expired at this time
            var expiredUploads = await this.imageUploadDataService.GetExpiredUploads();

            if (expiredUploads.Count != 0)
            {
                this.logger.LogInformation("Found {Count} expired uploads", expiredUploads.Count);

                foreach (var upload in expiredUploads)
                {
                    await this.Delete(upload);
                }
            }
            else
            {
                this.logger.LogInformation("No expired uploads to remove");
            }
        }
    }

    private async Task Delete(ImageUploadModel uploadDb)
    {
        try
        {
            this.logger.LogInformation("Deleting a record with id {Id} and expiration date {Date}",
                uploadDb.Id,
                uploadDb.ExpiresAtUtc);

            // Delete files
            this.fileService.Delete(uploadDb.GetAllFilePaths());

            // Delete a DB record
            await this.imageUploadDataService.Delete(uploadDb.Id);
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, ex.Message);
        }
    }
}