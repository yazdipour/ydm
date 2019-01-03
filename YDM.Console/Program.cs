using static System.Console;
using System.Threading.Tasks;
using YDM.Share;
using System.Linq;
using System.Collections.Generic;
using System;

namespace YDM.Console
{
    class Program
    {
        private static ApiHandler apiHandler = new ApiHandler();
        private static string[] Args;

        //private static string FindValueOfArg(string arg) => Args[Array.FindIndex(Args, (string _) => _ == arg) + 1];

        static void Main(string[] args)
        {
            Args = args;
            var features = new[] { "get", "search", "list", "setbase" };
            WriteLine("Welcome to Youtube Download Manager");
            WriteLine($">> ydm {features[0]} [VIDEO_URL or VIDEO_ID]");
            WriteLine($">> ydm {features[1]} [SEARCH_QUERY]");
            WriteLine($">> ydm {features[2]} [LIST_URL]");
            WriteLine($">> ydm {features[3]} [SERVER_URL]");
            WriteLine("\n");
            HandleFeatures(features);
            ReadKey();
        }

        private async static void HandleFeatures(string[] features)
        {
            await apiHandler.InitApi();
            if (features[0] == Args[0]) await GetVideo();
            else if (features[1] == Args[0]) await DoSearch();
            else if (features[2] == Args[0]) await GetList();
            else if (features[3] == Args[0]) await SetBase();
            WriteLine("\n[ENTER ANY KEY]");
        }

        private async static Task DoSearch()
        {
            WriteLine($"VideoId\t\tTitle");
            var items = await apiHandler.Api?.Search(Args[1], 10);
            foreach (var item in items) WriteLine($"{item.Id}\t{item.Title}");
        }

        private async static Task GetList()
        {
            var items = await apiHandler.Api.GetPlayListItems(Args[1]);
            WriteLine("List: " + Args[1]);
            WriteLine($"\nVideoId\t\tTitle");
            foreach (var item in items)
                WriteLine($"{item.Id}\t{item.Title}");
        }

        private async static Task SetBase()
        {
            var newUrl = Args[1] ?? "https://ydm.herokuapp.com";
            apiHandler.BASE_URL.SetBaseUrl(newUrl);
            await apiHandler.BASE_URL.LoadBaseUrl();
            WriteLine("BaseUrl is set to: " + newUrl);
        }

        async static Task GetVideo()
        {
            string videoId = Args[1];
            var result = await apiHandler.Api.GetAvailableVideoLink(videoId);
            WriteLine($"Id:{result.Info.Id}\tTitle:{result.Info.Title}\n\n");
            WriteLine($"Quality\t\tURL");
            foreach (var link in result.Links)
                WriteLine($"{link.Quality}\t\t{apiHandler.Api.GetDownloadLink(result.Info.Id, link.Tag)}");
        }
    }
}
