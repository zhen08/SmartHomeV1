﻿using System;
namespace SmartVideoHub
{
    public static class Constants
    {
        public const string DocumentPostingUri = @"https://smartcloudmvc.azurewebsites.net/api/Document";

		public const string CloudStorageConnectionString = @"DefaultEndpointsProtocol=https;AccountName=smarth;AccountKey=V1xZ9P/YDp9D/vNsVsepsidbl0SHv+otUSNgNSiJhJFmW1b2ULxMoQ54P3CMyegLG6ERHfS7wUNTks8wDa3DgQ==;EndpointSuffix=core.windows.net";
        public const string EventHistoryContainer = @"eventhistory";

        public const string LocalStorageFolder = @"/usr/local/data";
        public const string InferenceFolder = @"/dev/shm";

        public const int VideoFPS = 1;

    }
}
