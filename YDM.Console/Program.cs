using static System.Console;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;

namespace YDM.Console
{
    class Program
    {
        private static Share.ApiHandler apiHandler;
        private static string[] Args;
        private const string DEFAULT_BASEURL = "http://shahriar.in/app/ydm";

        static void Main(string[] args)
        {
            Args = args;
            var features = new Dictionary<string, Func<Task>>
            {
                { "search", GetVideo },
                { "get", GetVideo },
                { "list", GetVideo },
            };
            WriteLine("Welcome to YDM \n ydm base-" + string.Join('-', features.Keys));
            apiHandler = new Share.ApiHandler(args.Contains("base") ? PrepareBaseUrl(FindValueOfArg("base")) : DEFAULT_BASEURL);
            HandleFeatures(features);
        }

        private async static void HandleFeatures(Dictionary<string, Func<Task>> features)
        {
            foreach (var feature in features)
                if (Args.Contains(feature.Key))
                {
                    WriteLine(">>a");
                    await feature.Value();
                    WriteLine("a<<");
                    break;
                }
        }

        private static string PrepareBaseUrl(string baseUrl)
        {
            if (baseUrl.Length < 3) return DEFAULT_BASEURL;
            if (!baseUrl.Contains("http")) baseUrl += "http://";
            return baseUrl;
        }

        static string FindValueOfArg(string arg)
        {
            var index = Array.FindIndex(Args, (string _) => _ == arg);
            return Args[index + 1];
        }

        async static Task GetVideo()
        {
            string videoId = FindValueOfArg("get");
            WriteLine($"Gathering {videoId} ...");
            var result = await apiHandler.Api.GetVideoDownloadLink(videoId);
            WriteLine(result.Item1);
            WriteLine(">>z");
        }
    }
}
