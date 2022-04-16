using System;
namespace AlutaApp.Models
{
    public class HubConnection
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string ConnectionId { get; set; }

        public bool Connected { get; set; }

        public DateTime CreatedAt { get; set; }

        public HubConnection()
        {
        }
    }
}
