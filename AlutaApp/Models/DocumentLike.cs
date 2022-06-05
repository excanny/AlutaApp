using System;
namespace AlutaApp.Models
{
    public class DocumentLike
    {
        public int Id { get; set; }

        public int DocumentId { get; set; }

        public int UserId { get; set; }

        public DateTime TimeCreated { get; set; }

        public DocumentLike()
        {
        }
    }
}
