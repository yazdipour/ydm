using ytdl.Models;

namespace ydtl.android.Classes
{
	sealed partial class App
	{
		private static User usr;
		internal static int Today = 0;
		internal static User Usr
		{
			get { return usr; }
			set { usr = value; }
		}
	}
}