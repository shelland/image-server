// Created on 08/02/2021 20:04 by Andrey Laserson

namespace Shelland.ImageServer.Core.Models.Preferences
{
    public class CorsSettingsModel
    {
        public bool IsEnabled { get; set; }

        public string AllowedDomain { get; set; }
    }
}