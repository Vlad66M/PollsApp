using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PollsApp.Domain
{
    [Table("polls")]
    public class Poll
    {
        [Column("id")]
        public long Id { get; set; }

        [Column("title")]
        public string Title { get; set; }

        [Column("start_date")]
        public DateTime? StartDate { get; set; }

        [Column("end_date")]
        public DateTime? EndDate { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; }

        [Column("allow_comments")]
        public bool AllowComments { get; set; }

        public List<Comment> Comments { get; set; } = new();

        public List<PollOption> PollOptions { get; set; } = new();
    }
}
