using AlutaApp.Models.AlutaApp.Models;
using System;
namespace AlutaApp.Models
{
    public class Message
    {

        public int Id { get; set; }

        public string Content { get; set; }

        public int SenderId { get; set; }
        public User Sender { get; set; }

        public int RecieverId { get; set; }
        public User Reciever { get; set; }

        public bool Delivered { get; set; }

        public bool Read { get; set; }

        public DateTime TimeCreated { get; set; }

        public bool Deleted { get; set; }

        public Message()
        {
        }
    }
}
