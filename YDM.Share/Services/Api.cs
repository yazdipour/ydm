using System;
using System.Threading.Tasks;
using System.Reactive.Linq;
using Akavache;
using Newtonsoft.Json;
using YDM.Share.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace YDM.Share
{
    public class Api
    {
        private readonly string BASE_URL;

        public Api(string baseUrl) => this.BASE_URL = baseUrl;

        private string GET_VIDEO_URL(string videoId, string tag = null)
            => $"{BASE_URL}/getvideo.php?i={videoId}" + (tag == null ? "" : $"&tag={tag}");
        private string SEARCH_URL(string query, int max)
            => $"{BASE_URL}/search/search.php?q={query}&maxResults={max}";
        private string PLAYLIST_URL(string listId)
            => $"{BASE_URL}/search/playList.php?q={listId}";

        public string GetDownloadLink(string videoId, string tag) => GET_VIDEO_URL(videoId, tag);

        private IObservable<T> Request<T>(string url)
            => Observable.FromAsync(() => new System.Net.Http.HttpClient().GetAsync(url))
                              .SelectMany(async x =>
                              {
                                  x?.EnsureSuccessStatusCode();
                                  return await x?.Content?.ReadAsStringAsync();
                              }).Select(content => JsonConvert.DeserializeObject<T>(content));

        public async Task<VideoResponse> GetAvailableVideoLink(string videoUrl, ObservableCollection<VideoItem> downloadHistory = null)
        {
            int index = videoUrl.IndexOf("?v=", StringComparison.Ordinal);
            videoUrl = index == -1 ? videoUrl : videoUrl.Substring(index + 3);
            var result = await Request<VideoResponse>(GET_VIDEO_URL(videoUrl));
            foreach (var link in result.Links) link.Subtext = $"{link.Type} | {link.Quality}";
            result.Info.Duration = ConvertDuration(result.Info.Duration);
            await BlobCache.LocalMachine.InsertObject(result.Info.Id, result.Links);
            if (downloadHistory != null)
            {
                downloadHistory.Add(result.Info);
                await BlobCache.LocalMachine.InsertObject("History", downloadHistory);
            }
            return result;
        }

        public async Task<VideoItem[]> Search(string query, int max)
            => await Request<VideoItem[]>(SEARCH_URL(query, max));

        public async Task<VideoItem[]> GetPlayListItems(string url)
        {
            if (url?.Trim()?.Length == 0) throw new Exception("Invalid Url");
            int index = url.IndexOf("list=", StringComparison.Ordinal);
            if (index != -1) url = url.Substring(index + 5);
            return await Request<VideoItem[]>(PLAYLIST_URL(url));
        }

        private string ConvertDuration(string time)
        {
            try { return string.Format("{0}:{1}", Convert.ToInt32(time) / 60, Convert.ToInt32(time) % 60); }
            catch { return "!"; }
        }

        public async Task<DownloadLink[]> GetDownloadableLinks(string id)
        {
            try
            {
                return await BlobCache.LocalMachine.GetObject<DownloadLink[]>(id);
            }
            catch
            {
                return (await Request<VideoResponse>(GET_VIDEO_URL(id)))?.Links;
            }
        }
    }
}