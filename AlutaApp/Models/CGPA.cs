using AlutaApp.Models.AlutaApp.Models;
using System;
namespace AlutaApp.Models
{
    public class CGPA
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }

        public int Level { get; set; }

        public int Semester { get; set; }

        public string CourseCode { get; set; }

        public string Grade { get; set; }

        public double GradePoint { get; set; }

        public double CreditUnit { get; set; }

        public CGPA()
        {
        }
    }
}
