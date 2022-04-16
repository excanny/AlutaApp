using System;
namespace AlutaApp.Models
{
    public class ForgotPassword
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public int Code { get; set; }

        public bool Active { get; set; }

        public ForgotPassword()
        {
        }
    }
}
