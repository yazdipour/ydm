using Newtonsoft.Json;
using YDM.Share.Models;

namespace YDM.Share
{
    public class YdmResponse
    {
        [JsonProperty("info")]
        public DownloadedItems Info;
        [JsonProperty("links")]
        public LinkItems[] Links;
    }
}