// Created on 16/02/2023 13:43 by shell

using System;

namespace Shelland.ImageServer.AppServices.Services.Abstract.Common;

public interface IDateService
{
    DateTime NowUtc { get; }
}