using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AlutaApp.Models
{
    public class Post
    {
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }


        [Required]
        public string Title { get; set; }

        [Required]
        public int UserId { get; set; }
        public AlutaApp.Models.User User { get; set; }

        public List<PostLike> Likes { get; set; }

        public List<Comment> Comments { get; set; }

        public DateTime TimeCreated { get; set; }

        public DateTime LastUpdated { get; set; }

        public List<PostImage> Images { get; set; }

        [DefaultValue(true)]
        public bool Active { get; set; }

        public Post()
        {
        }
    }
}
