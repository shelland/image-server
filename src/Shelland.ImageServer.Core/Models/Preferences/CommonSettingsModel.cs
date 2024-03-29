﻿// Created on 09/02/2021 22:32 by Andrey Laserson

namespace Shelland.ImageServer.Core.Models.Preferences;

public class CommonSettingsModel
{
    public string ServerUrl { get; set; } = string.Empty;

    public string? RoutePrefix { get; set; }

    public bool SaveOriginalFile { get; set; }

    public bool IsServerEnabled { get; set; }
}