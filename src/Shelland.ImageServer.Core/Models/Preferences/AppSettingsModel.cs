// Created on 14/02/2021 12:20 by Andrey Laserson

namespace Shelland.ImageServer.Core.Models.Preferences
{
    public class AppSettingsModel
    {
        public CommonSettingsModel Common { get; set; }

        public DirectorySettingsModel Directory { get; set; }

        public RateLimitingSettingsModel RateLimiting { get; set; }

        public WebHooksSettingsModel WebHooks { get; set; }

        public ImageProcessingSettingsModel ImageProcessing { get; set; }

        public StaticCacheSettingsModel StaticCache { get; set; }

        public OnDemandProcessingSettingsModel OnDemandProcessing { get; set; }
    }
}