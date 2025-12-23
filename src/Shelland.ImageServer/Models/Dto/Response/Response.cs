// Created on 16/02/2021 18:32 by Andrey Laserson

namespace Shelland.ImageServer.Models.Dto.Response;

/// <summary>
/// Base result wrapper
/// </summary>
/// <typeparam name="T"></typeparam>
public record Response<T>
(
    T Data
) : BaseResponse(true);