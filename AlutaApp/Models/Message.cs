using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlutaApp.Models
{
    public class Message
    {

        public int Id { get; set; }

        public string Content { get; set; }

        public string MediaLink { get; set; }

        public int SenderId { get; set; }
        public User Sender { get; set; }

        public int RecieverId { get; set; }
        public User Reciever { get; set; }

        public bool Delivered { get; set; }

        public bool Read { get; set; }

        public DateTime TimeCreated { get; set; }

        public bool Deleted { get; set; }

        public bool FirstArchived { get; set; }

        public bool SecondArchived { get; set; }

        public int? ParentMessageId { get; set; }
        public Message ParentMessage { get; set; }

        public int? PostId { get; set; }
        public Post Post { get; set; }

        public Message()
        {
        }
    }
}
