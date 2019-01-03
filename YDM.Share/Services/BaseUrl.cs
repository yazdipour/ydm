using System;
using System.Threading.Tasks;
using Akavache;
using System.Reactive.Linq;

namespace YDM.Share.Services
{
    public class BaseUrl
    {
        public string BASE_URL { get; private set; }

        public BaseUrl(string v) => BASE_URL = v;

        public override string ToString() => BASE_URL;

        public async void SetBaseUrl(string url)
            => await BlobCache.LocalMachine.InsertObject("BASEURL", url);

        public async Task<string> LoadBaseUrl()
        {
            try
            {
                BASE_URL = await BlobCache.LocalMachine.GetObject<string>("BASEURL");
            }
            catch { }
            return BASE_URL;
        }
    }
}
