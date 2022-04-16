using System;
namespace AlutaApp.Models
{
    public class PropmotionPayments
    {
        public int Id { get; set; }

        public int PromotionId { get; set; }
        public Promotion Promotion { get; set; }

        public DateTime DateInitiated { get; set; }

        public string TransactionId { get; set; }

        public string Channel { get; set; }

        public double Amount { get; set; }

        public int Status { get; set; }

        public PropmotionPayments()
        {
        }
    }
}
