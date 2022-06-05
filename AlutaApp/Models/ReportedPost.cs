using System;
namespace AlutaApp.Models
{
    public class ReportedPost
    {
        public int Id { get; set; }

        public int PostId { get; set; }
        public Post Post { get; set; }

        public int ReporterId { get; set; }
        public User Reporter { get; set; }

        public DateTime TimeCreated { get; set; }

        public ReportedPost()
        {
        }
    }
}
