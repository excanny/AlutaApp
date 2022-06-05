using System;
namespace AlutaApp.Models
{
	public class PostVideo
	{
        public int Id { get; set; }

        public int PostId { get; set; }

        public string VideoLink { get; set; }

        public PostVideo()
		{
		}
	}
}

