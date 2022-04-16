using System;
namespace AlutaApp.Models
{
    public class Connection
    {
        public int Id { get; set; }

        public int Follower { get; set; }

        public int Following { get; set; }

        public bool Accepted { get; set; }

        public DateTime DateInitiated { get; set; }

        public DateTime DateAccepted { get; set; }

        public bool Active { get; set; }

        public Connection()
        {
        }
    }
}
