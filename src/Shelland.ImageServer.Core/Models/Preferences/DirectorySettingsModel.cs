// Created on 08/02/2021 17:02 by Andrey Laserson

namespace Shelland.ImageServer.Core.Models.Preferences
{
    public class DirectorySettingsModel
    {
       public string WorkingDirectory { get; set; } = string.Empty;

       public string AppDataDirectory { get; set; } = string.Empty;

       public string CacheDirectory { get; set; } = string.Empty;
    }
}