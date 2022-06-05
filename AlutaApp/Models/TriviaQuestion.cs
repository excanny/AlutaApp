using System;
namespace AlutaApp.Models
{
    public class TriviaQuestion
    {
        public int Id { get; set; }

        public int? TriviaId { get; set; }

        public string Question { get; set; }

        public string Option1 { get; set; }

        public string Option2 { get; set; }

        public string Option3 { get; set; }

        public string Option4 { get; set; }

        public int CorrectOption { get; set; }

        public TriviaQuestion()
        {
        }
    }
}
