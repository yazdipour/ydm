namespace YDM.Share.Models
{
    class DownloadedItems
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Duration { get; set; }
        public string Img => "https://shahriar.in/app/ydm/dl/getimage.php?i=" + Id;
        public string Views { get; set; }
        public string Channel { get; set; }
    }
}
