using System;
using System.ComponentModel.DataAnnotations;

namespace AlutaApp.Models
{
    public class Institution
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Abbreviation { get; set; }

        public Institution()
        {
        }
    }
}
