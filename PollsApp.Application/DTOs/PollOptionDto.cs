using PollsApp.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PollsApp.Application.DTOs
{
    public class PollOptionDto
    {

        
        public long Id { get; set; }

        
        public string? Text { get; set; }

        public byte[]? Photo { get; set; }

        public byte[]? Audio { get; set; }

        
        public long PollId { get; set; }

        
        public PollDto Poll { get; set; }

        public List<VoteDto> Votes { get; set; } = new();
    }
}
