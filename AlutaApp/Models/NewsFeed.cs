using System;
namespace AlutaApp.Models
{
    public class NewsFeed
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int PostId { get; set; }
        public Post Post { get; set; }

        public int PostRank { get; set; }

        public DateTime LastRanked { get; set; }

        public NewsFeed()
        {
        }
    }
}
