using AlutaApp.Models.AlutaApp.Models;
using System;
namespace AlutaApp.Models
{
    public class ReportedUser
    {
        public int Id { get; set; }

        public int ReporterId { get; set; }
        public User Reporter { get; set; }

        public int ReportedId { get; set; }
        public User Reported { get; set; }

        public string Comment { get; set; }

        public DateTime TimeCreated { get; set; }

        public ReportedUser()
        {
        }
    }
}
