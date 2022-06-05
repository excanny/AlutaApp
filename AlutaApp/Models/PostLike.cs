using System;
using System.ComponentModel.DataAnnotations;

namespace AlutaApp.Models
{
    public class PostLike
    {
        public int Id { get; set; }

        [Required]
        public int PostId { get; set; }

        [Required]
        public int UserId { get; set; }

        public DateTime TimeCreated { get; set; }

        public PostLike()
        {
        }
    }
}
