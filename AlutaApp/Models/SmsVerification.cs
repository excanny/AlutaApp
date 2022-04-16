using System;
using System.ComponentModel.DataAnnotations;

namespace AlutaApp.Models
{
    public class SmsVerification
    {
        public int Id { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string VerificationCode { get; set; }

        public SmsVerification()
        {
        }
    }
}
