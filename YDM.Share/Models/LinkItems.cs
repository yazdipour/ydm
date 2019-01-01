using Newtonsoft.Json;

namespace YDM.Share.Models
{
    public class LinkItems
    {
        [JsonProperty("str")]
        public string Subtext { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("quality")]
        public string Quality { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("tag")]
        public string Tag { get; set; }
    }
}
