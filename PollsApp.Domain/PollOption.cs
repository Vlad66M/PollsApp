using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PollsApp.Domain
{
    [Table("poll_options")]
    public class PollOption
    {
        [Column("id")]
        public long Id { get; set; }

        [Column("text")]
        public string Text { get; set; }

        [Column("poll_id")]
        public long PollId { get; set; }

        [JsonIgnore]
        public Poll Poll { get; set; }

        public List<Vote> Votes { get; set; } = new();

    }
}
