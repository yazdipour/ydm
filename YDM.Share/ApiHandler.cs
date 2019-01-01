using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Akavache;
using System.Reactive.Linq;
using YDM.Share.Models;
using System.Threading.Tasks;
using System.Linq;

namespace YDM.Share
{
    public class ApiHandler
    {
        public Api Api;

        public ObservableCollection<DownloadedItems> DownloadHistory = new ObservableCollection<DownloadedItems>();

        public ApiHandler(string baseUrl)
        {
            Akavache.Registrations.Start("YDM");
            Api = new Api(baseUrl);
        }

        public async Task<string> GetAllVideoLinkAsync(string quality = "high")
        {
            var links = new List<string>();
            var history = DownloadHistory.ToList();
            foreach (var dl in history)
            {
                try
                {
                    var list = await BlobCache.LocalMachine.GetObject<List<LinkItems>>(dl?.Id);
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
                var history = await BlobCache.LocalMachine.GetObject<DownloadedItems[]>("History");
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
    }
}
