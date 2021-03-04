// Created on 11/02/2021 16:30 by Andrey Laserson

namespace Shelland.ImageServer.Core.Other
{
    public class Constants
    {
        public const int DefaultJpegQuality = 100;
        public const int DefaultCacheDuration = 86400;
        public const int ExpiredUploadsServiceRunInterval = 60; // seconds
        public const int DefaultWatermarkOpacity = 50; // %s

        public const string AppDatabaseName = "AppDatabase.db";
        public const string Base64ImagePrefix = "data:image/jpg;base64,";
    }
}