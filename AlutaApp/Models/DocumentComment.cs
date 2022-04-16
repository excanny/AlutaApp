using AlutaApp.Models.AlutaApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AlutaApp.Models
{
    public class DocumentComment
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }
        public User User { get; set; }

        [Required]
        public int DocumentId { get; set; }

        public List<DocumentCommentLike> Likes { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime TimeCreated { get; set; }

        public DocumentComment()
        {
        }
    }
}
