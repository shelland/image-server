// Created on 10/02/2021 17:25 by Andrey Laserson

namespace Shelland.ImageServer.Core.Models.Enums
{
    public enum AppFlowExceptionType
    {
        GenericError,
        DiskWriteFailed,
        InvalidImageFormat,
        InvalidParameters,
        NetworkCallFailed,
        MalformedRequest
    }
}