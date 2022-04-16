using AlutaApp.Models.AlutaApp.Models;
using System;
namespace AlutaApp.Models
{
    public class TriviaWinner
    {
        public int Id { get; set; }

        public int TriviaId { get; set; }
        public Trivia? Trivia { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public double TotalPoints { get; set; }

        public int Position { get; set; }

        public TriviaWinner()
        {
        }
    }
}
