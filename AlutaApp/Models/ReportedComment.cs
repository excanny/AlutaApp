using System;
namespace AlutaApp.Models
{
    public class ReportedComment
    {
        public int Id { get; set; }

        public int CommentId { get; set; }
        public Comment Comment { get; set; }

        public int ReporterId { get; set; }
        public User Reporter { get; set; }

        public DateTime TimeCreated { get; set; }

        public ReportedComment()
        {
        }
    }
}
