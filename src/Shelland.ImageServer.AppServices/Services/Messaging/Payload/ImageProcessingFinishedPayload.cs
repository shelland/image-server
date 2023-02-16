// Created on 11/02/2021 20:20 by Andrey Laserson

using MediatR;
using Shelland.ImageServer.Core.Models.Domain;

namespace Shelland.ImageServer.AppServices.Services.Messaging.Payload;

public record ImageProcessingFinishedPayload(ImageUploadResultModel Result) : IRequest;