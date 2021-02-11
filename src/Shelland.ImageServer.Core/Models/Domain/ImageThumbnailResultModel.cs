// Created on 09/02/2021 15:07 by Andrey Laserson

namespace Shelland.ImageServer.Core.Models.Domain
{
    public class ImageThumbnailResultModel
    {
        public int Width { get; set; }

        public int Height { get; set; }

        public string DiskPath { get; set; }

        public string Url { get; set; }
    }
}