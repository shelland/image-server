// Created on 08/02/2021 16:41 by Andrey Laserson

using System;

namespace Shelland.ImageServer.Core.Models.Base
{
    public class BaseDbModel
    {
        public int Id { get; set; }

        public DateTimeOffset CreateDate { get; set; }

        public bool IsActive { get; set; }
    }
}