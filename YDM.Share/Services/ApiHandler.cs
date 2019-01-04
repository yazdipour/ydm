using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Akavache;
using System.Reactive.Linq;
using YDM.Share.Models;
using System.Threading.Tasks;
using System.Linq;
using YDM.Share.Services;

namespace YDM.Share
{
    public class ApiHandler
    {
        public Api Api;
        public BaseUrl BASE_URL = new BaseUrl("https://ydm.herokuapp.com");
        public readonly ObservableCollection<VideoItem> DownloadHistory
                    = new ObservableCollection<VideoItem>();

        public ApiHandler() => Registrations.Start("YDM");

        public async Task InitApi() => Api = new Api(await BASE_URL.LoadBaseUrl());

        public async Task<string> GetAllVideoLinkAsync(string quality = "high")
        {
            var links = new List<string>();
            var history = DownloadHistory.ToList();
            foreach (var dl in history)
            {
                try
                {
                    var list = await BlobCache.LocalMachine.GetObject<List<DownloadLink>>(dl?.Id);
                    links.Add(((quality != "high") ? list.Find(o => o.Quality.Contains(quality)) : list[0])?.Url);
                }
                catch { }
            }
            return string.Join(Environment.NewLine, links.ToArray());
        }

        public async Task LoadVideoHistory()
        {
            try
            {
                var history = await BlobCache.LocalMachine.GetObject<VideoItem[]>("History");
                DownloadHistory.Clear();
                foreach (var h in history) DownloadHistory.Add(h);
            }
            catch { }
        }


        public async Task<string> GetSizeFromUrl(string url)
        {
            try
            {
                System.Net.HttpWebRequest req = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
                req.Method = "HEAD";
                req.ContinueTimeout = 1000;
                var resp = await req.GetResponseAsync();
                float len = (float)resp.ContentLength / (1024 * 1024);
                return len.ToString("0.00") + " MB";
            }
            catch { return "!"; }
        }

        public async Task<Uri> GetRedirectedUri(string url)
        {
            try
            {
                System.Net.HttpWebRequest req = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
                req.Method = "HEAD";
                req.ContinueTimeout = 1000;
                var resp = await req.GetResponseAsync();
                return resp.ResponseUri;
            }
            catch { return new Uri(url); }
        }

        public async void RemoveHistoryItem(VideoItem item)
        {
            DownloadHistory.Remove(item);
            await BlobCache.LocalMachine.InsertObject("History", DownloadHistory);
        }
    }
}
