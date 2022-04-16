using System;
namespace AlutaApp.Models
{
    public class DocumentLike
    {
        public int Id { get; set; }

        public int DocumentId { get; set; }

        public int UserId { get; set; }

        public DocumentLike()
        {
        }
    }
}
