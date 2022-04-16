using System;
using AlutaApp.Models.AlutaApp.Models;
namespace AlutaApp.Models
{
    public class StatusView
    {
        public int Id { get; set; }

        public int? StatusId { get; set; }
        public Status? Status { get; set; }

        public int? UserId { get; set; }
        public User? User { get; set; }  
        public StatusView()
        {
        }
    }
}
