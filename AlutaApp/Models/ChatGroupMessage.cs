using AlutaApp.Models.AlutaApp.Models;
using System;
using System.Collections.Generic;

namespace AlutaApp.Models
{
    public class ChatGroupMessage
    {
        public int Id { get; set; }

        public int ChatGroupId { get; set; }

        public int UserId { get; set; }
        public User Sender { get; set; }

        public string Content { get; set; }

        public bool Deleted { get; set; }

        public DateTime TimeCreated { get; set; }

        public ChatGroupMessage()
        {
        }
    }

    public class GroupChatList
    {
       
        public List<int> ChatGroupId { get; set; }
        public List<string> ChatGroupName { get; set; }
        public List<MessageInfo> MessageInfos { get; set; }
    }

    public class MessageInfo
    {
        public string Contents { get; set; }
        public string Senders { get; set; }
        public DateTime TimeCreated { get; set; }
    }
}
