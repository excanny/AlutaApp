using System;
using System.Collections.Generic;

namespace AlutaApp.Models
{
    public class ChatGroupUser
    {
        public int Id { get; set; }

        public int ChatGroupId { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public DateTime DateJoined { get; set; }

        public bool Active { get; set; } = true;

        public ChatGroupUser()
        {
        }
    }
}
