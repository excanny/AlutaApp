namespace AlutaApp.Models
{
    public class ChatGroupMessage
    {
        public int Id { get; set; }

        public int ChatGroupId { get; set; }

        public int UserId { get; set; }
        public User Sender { get; set; }
        
        public string Content { get; set; }

        public string MediaLink { get; set; }

        public int? ParentMessageId { get; set; }
        public ChatGroupMessage ParentMessage { get; set; }

        public bool Deleted { get; set; }

        public DateTime TimeCreated { get; set; }

        public ChatGroupMessage()
        {
        }
    }
}
