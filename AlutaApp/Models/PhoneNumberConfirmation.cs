using System;
namespace AlutaApp.Models
{
    public class PhoneNumberConfirmation
    {
        public int Id { get; set; }

        public string Phone { get; set; }

        public int Code { get; set; }

        public bool Completed { get; set; }

        public bool Active { get; set; }

        public PhoneNumberConfirmation()
        {
        }
    }
}
