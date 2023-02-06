// Created on 03/02/2023 16:11 by shell

using Shelland.ImageServer.Core.Models.Domain;

namespace Shelland.ImageServer.FaceDetection.Services.Abstract;

public interface IFaceDetectionService
{
    Task<IReadOnlyCollection<FaceDetectionResultModel>> GetFaces(Stream imgStream, bool saveDetectedFaces, CancellationToken cancellationToken);
}