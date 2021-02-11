// Created on 09/02/2021 22:32 by Andrey Laserson

namespace Shelland.ImageServer.Core.Models.Preferences
{
    public class AppSettingsModel
    {
        public string ServerUrl { get; set; }

        public string RoutePrefix { get; set; }

        public bool SaveOriginalFile { get; set; }
    }
}