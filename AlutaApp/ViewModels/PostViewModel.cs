using AlutaApp.Models;
using AlutaApp.Models.AlutaApp.Models;
using System;
using System.Collections.Generic;

namespace AlutaApp.ViewModels
{
    public class PostViewModel
    {
        public int Id { get; set; }

        
        public string Content { get; set; }

        
        public string UserName { get; set; }
        public User User { get; set; }

        public List<PostLike> Likes { get; set; }

        public List<Comment> Comments { get; set; }

        public DateTime TimeCreated { get; set; }

        public DateTime LastUpdated { get; set; }

        public List<PostImage> Images { get; set; }

       
        public bool Active { get; set; }

    }

}
