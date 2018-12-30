using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Reactive.Linq;
using Akavache;
using Newtonsoft.Json;
using YDM.Share.Models;

namespace YDM.Share
{
    public class Api
    {
        private readonly string BASE_URL;

        public Api(string baseUrl) => this.BASE_URL = baseUrl;

        private string GET_VIDEO_URL(string videoId, string tag = null)
            => $"{BASE_URL}/dl/getvideo.php?i={videoId}&format={tag}";
        private string SEARCH_URL(string query, int max)
            => $"{BASE_URL}/search/search.php?q={query}&maxResults={max}";
        private string PLAYLIST_URL(string listId)
            => $"{BASE_URL}/search/playList.php?q={listId}";

        public async Task<DownloadedItems> GetVideoDownloadLink(List<DownloadedItems> downloadHistory, string videoUrl, string tag = null)
        {
            int index = videoUrl.IndexOf("?v=", StringComparison.Ordinal);
            videoUrl = index == -1 ? videoUrl : videoUrl.Substring(index + 3);
            var existing = downloadHistory.Find(obj => obj.Id == videoUrl);
            if (existing != null) return existing;
            var respondJson = await Request(GET_VIDEO_URL(videoUrl, tag));
            if (respondJson?.Length < 7) throw new Exception("Null Response");
            var respondObject = JsonConvert.DeserializeAnonymousType(respondJson, new { ItemInfo = new DownloadedItems(), DownloadLinks = new LinkItems[0] });
            foreach (var link in respondObject.DownloadLinks) link.str = $"{link.type} | {link.quality}";
            var ItemInfo = respondObject.ItemInfo;
            ItemInfo.Duration = ConvertDuration(ItemInfo.Duration);
            downloadHistory.Add(ItemInfo);
            await BlobCache.LocalMachine.InsertObject("MainList", downloadHistory);
            await BlobCache.LocalMachine.InsertObject(ItemInfo.Id, respondObject.DownloadLinks);
            return ItemInfo;
        }

        public async Task<DownloadedItems[]> Search(string query, int max)
            => JsonConvert.DeserializeObject<DownloadedItems[]>(await Request(SEARCH_URL(query, max)));

        public async Task<DownloadedItems[]> GetPlayListItems(string url)
        {
            int index = url.IndexOf("list=", StringComparison.Ordinal);
            if (index == -1) throw new Exception();
            url = url.Substring(index + 5);
            return JsonConvert.DeserializeObject<DownloadedItems[]>(await Request(PLAYLIST_URL(url)));
        }

        private async Task<string> Request(string url)
            => System.Text.Encoding.UTF8.GetString(await BlobCache.LocalMachine.DownloadUrl(url));

        private string ConvertDuration(string time)
        {
            try { return string.Format("{0}:{1}", Convert.ToInt32(time) / 60, Convert.ToInt32(time) % 60); }
            catch { return "!"; }
        }
    }
}