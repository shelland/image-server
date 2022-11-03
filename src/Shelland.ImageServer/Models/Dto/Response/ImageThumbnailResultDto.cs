// Created on 09/02/2021 15:07 by Andrey Laserson

namespace Shelland.ImageServer.Models.Dto.Response
{
    public class ImageThumbnailResultDto
    {
        public int Width { get; set; }

        public int Height { get; set; }

        public string DiskPath { get; set; } = string.Empty;

        public string Url { get; set; } = string.Empty;
    }
}