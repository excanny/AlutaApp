using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlutaApp.Models
{
    public class BlockedUser
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }

        [ForeignKey("UserBlockedId")]
        public int UserBlockedId { get; set; }
        public virtual User UserBlocked { get; set; }

        public DateTime TimeCreated { get; set; }

        public BlockedUser()
        {
        }
    }
}
