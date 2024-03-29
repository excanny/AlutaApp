﻿using System;
using System.Collections.Generic;

namespace AlutaApp.Models
{
    public class Status
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public string ImageLink { get; set; }

        public string Text { get; set; }

        public string TextColorCode { get; set; }

        public DateTime TimeCreated { get; set; }

        public List<StatusView> Views { get; set; }

        public Status()
        {
        }
    }
}
