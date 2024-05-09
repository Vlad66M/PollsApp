using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PollsApp.Domain
{
    [Table("comments")]
    public class Comment
    {
        [Column("id")]
        public long Id { get; set; }

        [Column("poll_id")]
        public long PollId { get; set; }

        [Column("user_id")]
        public string UserId { get; set; }

        [Column("text")]
        public string Text { get; set; }

        /*[JsonIgnore]*/
        public User User { get; set; }

        [JsonIgnore]
        public Poll Poll { get; set; }
    }
}
