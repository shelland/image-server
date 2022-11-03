// Created on 31/05/2021 19:49 by Andrey Laserson

using System;
using System.Collections.Generic;
using Shelland.ImageServer.Core.Models.Base;

namespace Shelland.ImageServer.Core.Models.Domain
{
    public class ProcessingProfileModel : BaseModel
    {
        public ProcessingProfileModel()
        {
            this.ProfileId = Guid.NewGuid();
        }

        public string Name { get; set; } = string.Empty;

        public List<ImageThumbnailParamsModel> Parameters { get; set; } = new();
        
        public Guid ProfileId { get; set; }
    }
}