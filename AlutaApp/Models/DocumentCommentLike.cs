using System;
using System.ComponentModel.DataAnnotations;

namespace AlutaApp.Models
{
    public class DocumentCommentLike
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int DocumentCommentId { get; set; }

        public DateTime TimeCreated { get; set; }

        public DocumentCommentLike()
        {
        }
    }
}
