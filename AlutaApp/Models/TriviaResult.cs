using AlutaApp.Models.AlutaApp.Models;
using System;
namespace AlutaApp.Models
{
    public class TriviaResult
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }

        public int TriviaId { get; set; }
        public Trivia? Trivia { get; set; }
        public int QuestionId { get; set; }
        public TriviaQuestion? TriviaQuestion { get; set; }
        public int OptionSelected { get; set; }

        public double TimeTakenInSeconds { get; set; }

        public TriviaResult()
        {
        }
    }
}
