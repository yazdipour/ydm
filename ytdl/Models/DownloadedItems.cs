namespace ytdl.Models
{
	class DownloadedItems
	{
		public string Id { get; set; }
		public string Title { get; set; }
		public string Duration { get; set; }
		public string Img => "http://shahriar.in/app/ytdlr/dl/getimage.php?i=" + Id;
		public string Views { get; set; }
	}
}
