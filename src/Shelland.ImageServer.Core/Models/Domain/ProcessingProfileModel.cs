// Created on 31/05/2021 19:49 by Andrey Laserson

using System;
using System.Collections.Generic;
using Shelland.ImageServer.Core.Models.Base;

namespace Shelland.ImageServer.Core.Models.Domain;

public class ProcessingProfileModel : BaseModel<Guid>
{
    public string Name { get; set; } = string.Empty;

    public IReadOnlyCollection<ImageThumbnailParamsModel> Parameters { get; set; } = null!;
}