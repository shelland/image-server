// Created on 31/05/2021 19:44 by Andrey Laserson

using System;
using System.Collections.Generic;
using Shelland.ImageServer.Core.Models.Base;
using Shelland.ImageServer.Core.Models.Domain;

namespace Shelland.ImageServer.Core.Models.Data;

public class ProcessingProfileDbModel : BaseModel<Guid>
{
    public string Name { get; set; } = string.Empty;

    public IReadOnlyCollection<ImageThumbnailParamsModel> Parameters { get; set; } = null!;
}