using Microsoft.AspNetCore.Mvc.Rendering;

namespace AlutaApp.ViewModels
{
    public class NotificationViewModel
    {
        public int NotificationId { get; set; }
        public string Content { get; set; }
        public bool Clicked { get; set; }
        public bool View { get; set; }
        public DateTime TimeCreated { get; set; }
        public List<SelectListItem>? Users { set; get; }
    }


    public class ListOfSenders
    {
      public string Sender {get; set;}
      public string SenderLastMessage {get; set;}
      public int MessageId { get; set; }

    }


    public class Likers
    {
      public string Name {get; set;}
      public DateTime TimeLiked {get; set;}
    }
    public class SentMessagesViewModel
    {
        public int Content { get; set; }
        public string SenderId { get; set; }
        public string Sender { get; set; }
        public string RecieverId { get; set; }
        public string Reciever { get; set; }
        public DateTime DateSent { get; set; }
        public bool  IsDelivered { get; set; }
        public bool IsRead { get; set; }
    }


}
