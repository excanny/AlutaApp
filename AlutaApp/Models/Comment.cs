using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AlutaApp.Models
{
    public class Comment
    {
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public DateTime TimeCreated { get; set; }

        [Required]
        public User User { get; set; }
        public int UserId { get; set; }

        [Required]
        public int PostId { get; set; }

        public int? CommentId { get; set; }

        public bool Active { get; set; }

        public List<CommentLike> CommentLikes { get; set; }

        public List<Comment> Replies { get; set; }

        public Comment()
        {
        }
    }
}
