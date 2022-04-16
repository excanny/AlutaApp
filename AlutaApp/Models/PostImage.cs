using System;
namespace AlutaApp.Models
{
    public class PostImage
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public string ImageLink { get; set; }


        public PostImage()
        {
        }
    }
}
