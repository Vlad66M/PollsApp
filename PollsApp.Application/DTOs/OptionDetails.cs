﻿using PollsApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PollsApp.Application.DTOs
{
    public class OptionDetails
    {
        public PollOption PollOption { get; set; }
        public int Votes { get; set; }
        public List<User> Users { get; set; } = new();
        public bool IsChecked { get; set; }
    }
}
