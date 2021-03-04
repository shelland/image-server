// Created on 04/03/2021 12:37 by Andrey Laserson

using System.Threading.Tasks;
using Shelland.ImageServer.Core.Models.Other;

namespace Shelland.ImageServer.AppServices.Services.Abstract.Common
{
    public interface IImageWritingService
    {
        /// <summary>
        /// Writes an image to the destination paths using provided parameters
        /// </summary>
        /// <param name="savingParams"></param>
        /// <returns></returns>
        Task Write(ImageSavingParamsModel savingParams);
    }
}