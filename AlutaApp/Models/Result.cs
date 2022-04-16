using System;
using System.ComponentModel.DataAnnotations;

namespace AlutaApp.Models
{
    public class Result
    {
        public int Id { get; set; }

        [Required]
        public int Userid { get; set; }

        [Required]
        public int Level { get; set; }

        [Required]
        public int Semester { get; set; }

        [Required]
        public string CourseCode { get; set; }

        [Required]
        public int CourseCredit { get; set; }

        [Required]
        public char Grade { get; set; }

        public Result()
        {
        }
    }
}
