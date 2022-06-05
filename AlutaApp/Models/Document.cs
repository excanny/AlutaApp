using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AlutaApp.Models
{
    public class Document
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }
        public User User { get; set; }

        [Required]
        public int DepartmentId { get; set; }
        public Department Department { get; set; }

        [Required]
        public int CategoryId { get; set; }
        public DocumentCategory Category { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string DocumentLink { get; set; }

        public string DocumentThumbnailLink { get; set; }

        public int TotalDownloads { get; set; }

        public DateTime TimeCreated { get; set; }

        public DateTime LastUpdated { get; set; }

        public List<DocumentLike> Likes { get; set; }

        public List<DocumentComment> Comments { get; set; }

        public Document()
        {
        }
    }
}
