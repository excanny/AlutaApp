using System;
namespace AlutaApp.Models
{
    public class Notification
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Content { get; set; }

        public string Category { get; set; }

        public int CategoryId { get; set; }

        public int SecondPartyId { get; set; }

        public User SecondParty { get; set; }

        public int ThirdPartyId { get; set; }

        public bool Viewed { get; set; }

        public bool Clicked { get; set; }

        public DateTime TimeCreated { get; set; }

        public bool Active { get; set; }

        public Notification()
        {
        }
    }
}
