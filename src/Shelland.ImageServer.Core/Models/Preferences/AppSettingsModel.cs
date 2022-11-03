// Created on 14/02/2021 12:20 by Andrey Laserson

namespace Shelland.ImageServer.Core.Models.Preferences
{
    public class AppSettingsModel
    {
        public CommonSettingsModel Common { get; set; } = null!;

        public DirectorySettingsModel Directory { get; set; } = null!;

        public RateLimitingSettingsModel RateLimiting { get; set; } = null!;

        public WebHooksSettingsModel WebHooks { get; set; } = null!;

        public ImageProcessingSettingsModel ImageProcessing { get; set; } = null!;

        public StaticCacheSettingsModel StaticCache { get; set; } = null!;

        public OnDemandProcessingSettingsModel OnDemandProcessing { get; set; } = null!;
    }
}