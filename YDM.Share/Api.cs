using System;

namespace YDM.Share
{
    public class Api
    {
        public static string BASE_URL = "http://shahriar.in/app/ydm/";
        public static string GET_VIDEO(string token, string videoId) => $"/dl/getvideo.php?u={token}&i={videoId}";
        //Task<string> GetVideo(string token, string video);

        public static string GET_VIDEO(string query,int max) => $"/search/search.php?q={query}&maxResults={max}";
        //Task<List<DownloadedItems>> Search(string text, string max);

        public static string GET_VIDEO(string listId)=>$"/search/playList.php?q={listId}";
        //Task<List<DownloadedItems>> GetPlayList(string list);
    }
}
//        public static IYdm YdmApi = RestService.For<IYdm>(BASE_URL);
//        public static ObservableCollection<DownloadedItems> clist = new ObservableCollection<DownloadedItems>();
//        private static Boolean CheckIsConnect()
//        {
//            if (App.Today == 0)
//            {
//                Utils.ShowMSG("Error in connecting to server! Please restart the application");
//                return false;
//            }
//            return true;
//        }

//        private static Boolean CheckCharge()
//        {
//            if (App.Usr.nrCanDownload <= 0)
//                return false;
//            return CheckIsConnect();
//        }

//        private static string Token
//        {
//            get
//            {
//                var inside = (App.Usr.Id + "|" + App.Today);
//                string token = Utils.Base64Encode(Utils.Reverse(Utils.Base64Encode(inside)));
//                return token;
//            }
//        }

//        public static async Task<Boolean> MakeMyDayAsync()
//        {
//            try
//            {
//                string account = LocalSettingManager.ReadSetting("Account");
//                App.Usr = JsonConvert.DeserializeObject<User>(account);
//                string date = await YdmApi.GetDate(Utils.Base64Encode(Utils.Reverse(Utils.Base64Encode(App.Usr.Id.ToString()))));
//                date = Utils.MultiBase64Decode(date, 6);
//                if (date.Substring(0, 3).Equals("Err")) throw new Exception();
//                var arr = date.Split('|');
//                App.Today = Convert.ToInt32(arr[0]);
//                App.Usr.nrCanDownload = Convert.ToInt32(arr[1]);
//                if (App.Usr.nrCanDownload < 0) App.Usr.nrCanDownload = 0;
//                Views.MotherPanel.StaticNr.Label = App.Usr.nrCanDownload.ToString();
//                return true;
//            }
//            catch { return false; }
//        }

//        public async static Task<string> GetVideo(string input, bool AutoLoadingGUI = true, bool ShowMsg = true)
//        {
//            List<DownloadedItems> myList = new List<DownloadedItems>(clist);
//            var tempo = input;
//            if (input.Contains("?v=")) tempo = input.Substring(input.IndexOf("?v=") + 3);
//            var x = myList.Find(obj => obj.Id == tempo);
//            if (x != null)
//            {
//                if (ShowMsg) Utils.ShowMSG("این ویدیو در لیست موجود است");
//                return null;
//            }
//            if (!CheckCharge())
//            {
//                if (ShowMsg) Utils.ShowMSG("اکانت خود را شارژ کنید");
//                return null;
//            }
//            string key = null;
//            try
//            {
//                if (AutoLoadingGUI) Views.MotherPanel.StaticRing.IsLoading = true;
//                string video = Utils.Base64Encode(input);
//                var respond = await YdmApi.GetVideo(Token, video);
//                if (respond.Substring(0, 3) == "Err")
//                {
//                    Views.MotherPanel.StaticRing.IsLoading = false;
//                    Utils.ShowMSG(respond);
//                    return null;
//                }
//                var temp = respond.Split(new string[] { "[*]" }, StringSplitOptions.RemoveEmptyEntries);

//                var DLI = JsonConvert.DeserializeObject<DownloadedItems>(temp[0]);
//                clist.Insert(0, DLI);
//                DLI.Duration = ConvertDuration(DLI.Duration);
//                key = "LI" + DLI.Id;
//                var LI = JsonConvert.DeserializeObject<LinkItems[]>(temp[1]);
//                foreach (var item in LI)
//                    item.str = item.type + " | " + item.quality;
//                await BlobCache.LocalMachine.InsertObject("MainList", clist);
//                await BlobCache.LocalMachine.InsertObject(key, LI);
//                Views.MotherPanel.StaticNr.Label = App.Usr.nrCanDownload.ToString();
//                App.Usr.nrCanDownload--;
//            }
//            catch
//            {
//                if (ShowMsg) Utils.ShowMSG("Problem in getting your video! Please try later");
//            }
//            if (AutoLoadingGUI) Views.MotherPanel.StaticRing.IsLoading = false;
//            return key;
//        }

//        public static async Task<string> FillSizeAsync(string id, string link, int inx)
//        {
//            var key = id + "_" + inx;
//            try
//            {
//                var size = await BlobCache.LocalMachine.GetObject<string>(key);
//                return size;
//            }
//            catch (KeyNotFoundException)
//            {
//                var size = await GetSize(link);
//                await BlobCache.LocalMachine.InsertObject<string>(key, size);
//                return size;
//            }
//            catch
//            {
//                return "!";
//            }
//        }

//        public static string GetVideoLink(string id, string tag)
//            => BASE_URL + "/dl/getvideo.php?u=" + Token + "&i=" + Utils.Base64Encode(id) + "&format=" + tag;

//        public static async void GetAllVideoLinkAsync(string quality = "high")
//        {
//            var links = new List<string>();
//            List<DownloadedItems> ser = new List<DownloadedItems>();
//            try
//            {
//                foreach (var dl in await BlobCache.LocalMachine.GetObject<List<DownloadedItems>>("MainList"))
//                {
//                    try
//                    {
//                        var ls = await BlobCache.LocalMachine.GetObject<LinkItems[]>("LI" + dl.Id);
//                        var list = new List<LinkItems>(ls);
//                        var temp = ls[0];
//                        if (quality != "high") temp = list.Find(o => o.quality.Contains("medium"));
//                        if (temp == null) temp = ls[0];
//                        links.Add(GetVideoLink(dl.Id, temp.tag));
//                    }
//                    catch { }
//                }
//            }
//            catch { }
//            var linksString = string.Join(Environment.NewLine, links.ToArray());
//            var dataPackage = new DataPackage();
//            dataPackage.SetText(linksString);
//            Clipboard.SetContent(dataPackage);
//        }

//        public async static Task<List<DownloadedItems>> SearchVideo(string input, string maxRes)
//            => await YdmApi.Search(Utils.Reverse(Utils.Base64Encode(input)), Utils.Base64Encode(maxRes));

//        public async static Task<List<DownloadedItems>> GetPlayList(string str)
//        {
//            if (str.Contains("list=")) str = str.Substring(str.IndexOf("list=") + 5);
//            str = Utils.Reverse(Utils.Base64Encode(str));
//            return await YdmApi.GetPlayList(str);
//        }

//        public static async Task<string> GetSize(string url)
//        {
//            try
//            {
//                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
//                req.Method = "HEAD";
//                req.ContinueTimeout = 1000;
//                var resp = await req.GetResponseAsync();
//                float len = (float)resp.ContentLength / (1024 * 1024);
//                return len.ToString("0.00") + " MB";
//            }
//            catch { return "!"; }
//        }

//        private static string ConvertDuration(string drr)
//        {
//            try
//            {
//                int dr = Convert.ToInt32(drr);
//                return string.Format("{0}:{1}", dr / 60, dr % 60);
//            }
//            catch { return "!"; }
//        }

//        public static async void GetBatchOfVideos(List<string> urls)
//        {
//            Views.MotherPanel.StaticRing.IsLoading = true;
//            foreach (var item in urls)
//            {
//                try
//                {
//                    var key = await GetVideo(item, false, false);
//                    if (key == null) continue;
//                }
//                catch { }
//            }
//            Views.MotherPanel.StaticRing.IsLoading = false;
//        }
//    }
//}
