using System;
namespace YDM.Share.Models
{
    class User
    {
        public int Id { get; set; } = 0;
        public string Name { get; set; }
        public string Email { get; set; }
        public string Picture { get; set; } = "";
        public int nrCanDownload { get; set; } = 0;
    }
}
