using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PollsApp.Domain
{
    [Table("votes")]
    public class Vote
    {
        [Column("id")]
        public long Id { get; set; }

        [Column("poll_option_id")]
        public long PollOptionId { get; set; }

        [Column("user_id")]
        public string UserId { get; set; }

        [Column("is_anon")]
        public bool IsAnon { get; set; }

        [JsonIgnore]
        public PollOption PollOption { get; set; }

        [JsonIgnore]
        public User User { get; set; }

    }
}
