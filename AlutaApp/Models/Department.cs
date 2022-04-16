using System;
using System.ComponentModel.DataAnnotations;

namespace AlutaApp.Models
{
    public class Department
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public Department()
        {
        }
    }
}
