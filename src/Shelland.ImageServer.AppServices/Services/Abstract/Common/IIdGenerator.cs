// Created on 16/02/2023 15:21 by shell

using System;

namespace Shelland.ImageServer.AppServices.Services.Abstract.Common;

public interface IIdGenerator
{
    Guid Id();
}