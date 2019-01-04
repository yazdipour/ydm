namespace YDM.Share.Models
{
    public class VideoItem
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Duration { get; set; }
        public string Views { get; set; }
        public string Channel { get; set; }
        private string image;
        public string Image
        {
            get
            {
                return image ?? "https://ydm.herokuapp.com/getimage.php?i=" + Id;
            }
            set { image = value; }
        }

    }
}
