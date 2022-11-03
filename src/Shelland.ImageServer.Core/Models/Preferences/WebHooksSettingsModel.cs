// Created on 08/02/2021 20:04 by Andrey Laserson

namespace Shelland.ImageServer.Core.Models.Preferences
{
    public class WebHooksSettingsModel
    {
        public bool IsEnabled { get; set; }

        public string PostUrl { get; set; } = string.Empty;
    }
}