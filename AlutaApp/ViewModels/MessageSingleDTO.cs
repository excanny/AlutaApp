namespace AlutaApp.ViewModels
{
    public class MessageSingleDTO
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public string MediaLink { get; set; }

        public bool IsReel { get; set; }

        public ParentMessageDTO? ParentMessage { get; set; }

        public int? PostId { get; set; }

        public int SenderId { get; set; }

        public int RecieverId { get; set; }

        public bool Delivered { get; set; }

        public bool Read { get; set; }

        public DateTime TimeCreated { get; set; }

        public bool Deleted { get; set; }
    }
}
