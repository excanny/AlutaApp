using System;
namespace AlutaApp.Models
{
	public class PointsLog
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public double Points { get; set; }

        public string Reason { get; set; }

        public DateTime DateCreated { get; set; }
    }
}

