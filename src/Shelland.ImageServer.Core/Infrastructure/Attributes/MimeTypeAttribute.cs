// Created on 05/03/2021 11:12 by Andrey Laserson

using System.ComponentModel;
using JetBrains.Annotations;

namespace Shelland.ImageServer.Core.Infrastructure.Attributes
{
    public class MimeTypeAttribute : DescriptionAttribute
    {
        public MimeTypeAttribute([NotNull] string description) : base(description)
        {
        }
    }
}