using System;
namespace AlutaApp.Models
{
    public class StatusView
    {
        public int Id { get; set; }

        public int StatusId { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public StatusView()
        {
        }
    }
}
