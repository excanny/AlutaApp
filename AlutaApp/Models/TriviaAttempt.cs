using System;
namespace AlutaApp.Models
{
    public class TriviaAttempt
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int TriviaId { get; set; }

        public int TotalCorrectAnswers { get; set; }

        public double TotalTimeTakenInSeconds { get; set; }

        public double TotalScore { get; set; }

        public TriviaAttempt()
        {
        }
    }
}
