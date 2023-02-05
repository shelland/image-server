// Created on 03/02/2023 16:14 by shell

using System.Reflection;
using OpenCvSharp;
using Shelland.ImageServer.AppServices.Services.Abstract.Common;
using Shelland.ImageServer.AppServices.Services.Abstract.Storage;
using Shelland.ImageServer.Core.Infrastructure.Extensions;
using Shelland.ImageServer.Core.Models.Domain;
using Shelland.ImageServer.Core.Models.Enums;
using Shelland.ImageServer.FaceDetection.Services.Abstract;

namespace Shelland.ImageServer.FaceDetection.Services;

public class FaceDetectionService : IFaceDetectionService
{
    private readonly IFileService fileService;
    private readonly ILinkService linkService;

    public FaceDetectionService(IFileService fileService, ILinkService linkService)
    {
        this.fileService = fileService;
        this.linkService = linkService;
    }

    public async Task<IReadOnlyCollection<FaceRecognitionRectModel>> GetFaces(Stream imgStream, bool saveDetectedFaces,CancellationToken cancellationToken)
    {
        var results = new List<FaceRecognitionRectModel>();

        using var srcImage = Mat.FromStream(imgStream, ImreadModes.Unchanged);
        using var grayImage = new Mat();

        Cv2.CvtColor(srcImage, grayImage, ColorConversionCodes.BGRA2GRAY);
        Cv2.EqualizeHist(grayImage, grayImage);

        var currentPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().NotNull().Location).NotNull();

        using var cascade = new CascadeClassifier(Path.Combine(currentPath, "haarcascade_frontalface_alt.xml"));
        using var nestedCascade = new CascadeClassifier(Path.Combine(currentPath, "haarcascade_eye_tree_eyeglasses.xml"));

        var faces = await Task.Run(() => cascade.DetectMultiScale(
            image: grayImage,
            scaleFactor: 1.1,
            minNeighbors: 2,
            flags: HaarDetectionTypes.DoRoughSearch | HaarDetectionTypes.ScaleImage,
            minSize: new Size(30, 30)
        ), cancellationToken);

        foreach (var face in faces)
        {
            var outputInfo = new FaceRecognitionRectModel(
                face.TopLeft.X,
                face.TopLeft.Y,
                face.BottomRight.X,
                face.BottomRight.Y
            );

            if (saveDetectedFaces)
            {
                var path = this.fileService.PrepareStoragePath(OutputImageFormat.Jpeg);

                using var faceImage = new Mat(srcImage, face);
                using var imageStream = faceImage.ToMemoryStream(".jpg", new ImageEncodingParam(ImwriteFlags.JpegQuality, 100));

                await this.fileService.WriteFile(imageStream, path.FilePath, cancellationToken);

                outputInfo = outputInfo with
                {
                    ImageUrl = this.linkService.NormalizeWebPath(path.UrlPath)
                };
            }

            results.Add(outputInfo);
        }

        return results;
    }
}