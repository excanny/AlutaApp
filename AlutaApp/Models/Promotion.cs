using System;
namespace AlutaApp.Models
{
    public class Promotion
    {
        public int Id { get; set; }

        public int PostId { get; set; }
        public Post Post { get; set; }

        public int Views { get; set; }

        public int PromotedById { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int Status { get; set; }

        public Promotion()
        {
        }
    }
}
