// Created on 23/12/2025 18:49 by Laserson

namespace Shelland.ImageServer.Models.Dto.Response;

/// <summary>
/// Base error result wrapper
/// </summary>
public record ErrorResponse
(
    string Message,
    string? Path = null,
    string? Meta = null
) : BaseResponse(false);