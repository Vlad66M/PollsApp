using PollsApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PollsApp.Application.DTOs
{
    public class PollViewModel
    {
        public Poll Poll { get; set; }
        public User User { get; set; }
    }
}
