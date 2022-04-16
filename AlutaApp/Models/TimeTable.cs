using System;
namespace AlutaApp.Models
{
    public class TimeTable
    {
        public int Id { get; set; }

        public int? UserId { get; set; }
        public AlutaApp.Models.User? User { get; set; }

        public string Course { get; set; }

        public int Day { get; set; }

        public int Hour { get; set; }

        public int Minute { get; set; }

        public int RemindInMinutes { get; set; }

        public TimeTable()
        {
        }
    }
}
