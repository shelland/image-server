﻿// Created on 06/02/2023 18:28 by shell

namespace Shelland.ImageServer.AppServices.Services.Abstract.Common;

public interface ILinkService
{
    /// <summary>
    /// URL normalization (slashes, etc.)
    /// </summary>
    /// <param name="originalUrl"></param>
    /// <returns></returns>
    string PrepareWebPath(string originalUrl);
}