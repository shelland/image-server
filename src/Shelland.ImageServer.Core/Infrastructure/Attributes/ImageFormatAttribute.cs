// Created on 05/03/2021 11:13 by Andrey Laserson

using System.ComponentModel;

namespace Shelland.ImageServer.Core.Infrastructure.Attributes
{
    public class ImageFormatAttribute : DescriptionAttribute
    {
        public ImageFormatAttribute(string description) : base(description)
        {
        }
    }
}