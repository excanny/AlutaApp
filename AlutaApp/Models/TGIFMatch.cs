using System;
using System.Collections.Generic;

namespace AlutaApp.Models
{
    public class TGIFMatch
    {
        public int Id { get; set; }

        public int MaleId { get; set; }
        public User Male { get; set; }

        public int FemaleId { get; set; }
        public User Female { get; set; }

        public int MaleStatus { get; set; }

        public int FemaleStatus { get; set; }

        public DateTime DateMatched { get; set; }

        public DateTime DateOfExpiry { get; set; }

        public List<TGIFMessage> Messages { get; set; }

        public TGIFMatch()
        {
        }
    }
}
