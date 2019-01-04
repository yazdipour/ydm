using Newtonsoft.Json;
using YDM.Share.Models;

namespace YDM.Share
{
    public class VideoResponse
    {
        [JsonProperty("info")]
        public VideoItem Info;
        [JsonProperty("links")]
        public DownloadLink[] Links;
    }
}