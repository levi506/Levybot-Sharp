using Google.Apis.Services;
using Google.Apis.YouTube.v3;

namespace LevyBotSharp.Utility.Apis
{
    public static class YoutubeApi
    {
        public static YouTubeService Api { get; set; }

        public static void Build(string apiKey)
        {
            Api = new YouTubeService (new BaseClientService.Initializer()
            {
                ApiKey = apiKey,
                ApplicationName = "LevyBot"
            });

        }
    }
}