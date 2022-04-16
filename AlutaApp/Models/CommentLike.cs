using AlutaApp.Models.AlutaApp.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace AlutaApp.Models
{
    public class CommentLike
    {
        public int Id { get; set; }

        [Required]
        public int CommentId { get; set; }

        [Required]
        public int UserId { get; set; }
        public User User { get; set; }

        public DateTime TimeCreated { get; set; }

        public CommentLike()
        {
        }
    }
}
