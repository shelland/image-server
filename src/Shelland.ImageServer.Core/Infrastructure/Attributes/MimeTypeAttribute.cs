// Created on 05/03/2021 11:12 by Andrey Laserson

using System.ComponentModel;

namespace Shelland.ImageServer.Core.Infrastructure.Attributes
{
    public class MimeTypeAttribute : DescriptionAttribute
    {
        public MimeTypeAttribute(string description) : base(description)
        {
        }
    }
}