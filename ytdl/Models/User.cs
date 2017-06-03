namespace ytdl.Models {
    class User {
        public int Id { get; set; } = 0;
        public string Name { get; set; }
        public string Email { get; set; }
        public string Img { get; set; } = "";
        //public int expTime { get; set; }
        public int leftDay { get; set; } = -1;
    }
}
