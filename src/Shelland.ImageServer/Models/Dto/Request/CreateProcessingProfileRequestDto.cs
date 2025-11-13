// Created on 01/06/2021 19:19 by Andrey Laserson

using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Shelland.ImageServer.Infrastructure.ModelBinding;

namespace Shelland.ImageServer.Models.Dto.Request;

public record CreateProcessingProfileRequestDto(
    [BindRequired] string Name,
    [ModelBinder(BinderType = typeof(JsonBodyModelBinder))] IReadOnlyCollection<ImageThumbnailParamsDto> Parameters
);