using System.Collections.Generic;
using System.Threading.Tasks;
using Refit;
using ytdl.Models;

namespace ytdl.Classes
{
	interface IYdm
	{
		[Get("/auth/getuser.php?i={user}")]
		Task<string> GetUser(string user);

		[Get("/auth/logout.php")]
		Task Logout();
		
		[Get("/dl/getdate.php?i={user}")]
		Task<string> GetDate(string user);

		[Get("/dl/getvideo.php?u={token}&i={video}")]
		Task<string> GetVideo(string token,string video);

		[Get("/search/search.php?q={text}&maxResults={max}")]
		Task<List<DownloadedItems>> Search(string text,string max);

		[Get("/ydm/search/playList.php?q={list}")]
		Task<List<DownloadedItems>> GetPlayList(string list);
	}
}
