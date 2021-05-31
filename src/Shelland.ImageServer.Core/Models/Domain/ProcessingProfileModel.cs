// Created on 31/05/2021 19:49 by Andrey Laserson

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Shelland.ImageServer.Core.Models.Base;

namespace Shelland.ImageServer.Core.Models.Domain
{
    public class ProcessingProfileModel : BaseModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public List<ImageThumbnailParamsModel> Parameters { get; set; }
        
        public Guid ProfileId { get; set; }
    }
}