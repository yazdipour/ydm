using Akavache;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Reactive.Linq;

namespace ytdl.android.Classes
{
	static class AkavacheHelper
	{
		public static void Init()
		{
			BlobCache.ApplicationName = "YDM";
		}
		public async static Task SaveStringLocal(string key, string x)
		{
			await BlobCache.LocalMachine.InsertObject(key, x);
		}
		public async static Task<string> ReadStringLocal(string key)
		{
			try { return await BlobCache.LocalMachine.GetObject<string>(key); }
			catch (KeyNotFoundException) { return null; }
		}
		public async static Task<Boolean> ExistStringLocal(string key)
		{
			try { return await ReadStringLocal(key)==null ? false : true; }
			catch (KeyNotFoundException) { return false; }
		}
		public async static Task RemoveFromLocal(string key)
		{
			await BlobCache.LocalMachine.Invalidate(key);
		}
		public static void ShutDown()
		{
			BlobCache.Shutdown().Wait();
		}
		// To get List of Objects
		// To Remove all of them
	}
}
