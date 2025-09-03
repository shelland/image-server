// Created on 16/02/2023 15:22 by shell

using System;
using Shelland.ImageServer.AppServices.Services.Abstract.Common;

namespace Shelland.ImageServer.AppServices.Services.Common;

public class IdGenerator : IIdGenerator
{
    public Guid Id()
    {
        return Guid.CreateVersion7();
    }
}