using System;
namespace AlutaApp.Models
{
    public class TGIFMessage
    {
        public int Id { get; set; }

        public int TGIFMatchId { get; set; }

        public string Content { get; set; }

        public int SenderId { get; set; }
        public User Sender { get; set; }

        public int ReceiverId { get; set; }
        public User Receiver { get; set; }

        public DateTime TimeCreated { get; set; }

        public bool Delivered { get; set; }

        public bool Read { get; set; }

        public bool Deleted { get; set; }

        public TGIFMessage()
        {
        }
    }
}
