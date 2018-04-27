using System;
using Windows.UI.Popups;

namespace ytdl.Classes
{
	internal static class Utils
	{

		public static void ShowMSG(string msg)
		{
			var dialog = new MessageDialog(msg);
			dialog.Commands.Add(new UICommand("Close"));
		}
		
		public static string Base64Encode(string plainText)
		{
			var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
			return Convert.ToBase64String(plainTextBytes);
		}
		public static string Base64Decode(string base64EncodedData)
		{
			var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
			return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
		}
		public static string Reverse(string s)
		{
			char[] charArray = s.ToCharArray();
			Array.Reverse(charArray);
			return new string(charArray);
		}

		public static string MultiBase64Encode(string str, int n)
		{
			for (int i = 0; i < n; i++)
			{
				str = Base64Encode(str);
			}
			return str;
		}
		public static string MultiBase64Decode(string str, int n)
		{
			for (int i = 0; i < n; i++)
			{
				str = Base64Decode(str);
			}
			return str;
		}
	}
}
