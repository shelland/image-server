﻿// Created on 31/05/2021 19:44 by Andrey Laserson

using System;
using System.Collections.Generic;
using Shelland.ImageServer.Core.Models.Base;
using Shelland.ImageServer.Core.Models.Domain;

namespace Shelland.ImageServer.Core.Models.Data
{
    public class ProcessingProfileDbModel : BaseModel
    {
        public ProcessingProfileDbModel()
        {
            this.CreateDate = DateTimeOffset.UtcNow;
        }

        public string Name { get; set; }

        public List<ImageThumbnailParamsModel> Parameters { get; set; }

        public Guid ProfileId { get; set; }
    }
}