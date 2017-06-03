using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Windows.Networking.BackgroundTransfer;
using Windows.Networking.Connectivity;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Popups;

namespace ytdl.Classes {
    internal static class CloseHelp {
        public static async Task<string> DownloadPages(CancellationToken token,string url) {
            var http = new HttpClient();
            try {
                if(NetworkInformation.GetInternetConnectionProfile() == null)
                    return "Err: Internet Connection Error!";
                var response = await http.GetAsync(new Uri(url),token);
                await Task.Delay(1,token);
                response.EnsureSuccessStatusCode();
                var htmlFile = await response.Content.ReadAsStringAsync();
                if(htmlFile.Contains("Warning") || htmlFile == "")
                    return "Err: Server Side Error!";
                return htmlFile;
            }
            catch(TaskCanceledException) { throw; }
            catch { return "Err"; }
        }
        public static async Task<string> GetSize(string url) {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "HEAD";
            var resp = await req.GetResponseAsync();
            float len = (float)resp.ContentLength/(1024*1024);
            return len.ToString("0.00")+" MB";
        }
        public static string Base64Encode(string plainText) {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData) {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static string Reverse(string s) {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
    }
}
