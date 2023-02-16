// Created on 16/02/2023 13:43 by shell

using System;
using Shelland.ImageServer.AppServices.Services.Abstract.Common;

namespace Shelland.ImageServer.AppServices.Services.Common;

public class DateService : IDateService
{
    public DateTime NowUtc => DateTime.UtcNow;
}