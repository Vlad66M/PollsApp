using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PollsApp.Domain
{
    /*[Table("users")]*/
    public class User : IdentityUser
    {
        /*[Column("id")]
        public long Id { get; set; }*/

        [Column("name")]
        public string Name { get; set; }

        /*[Column("email")]
        public string Email { get; set; }*/

        [Column("password")]
        public string Password { get; set; }

        [Column("role_id")]
        public long RoleId { get; set; }
        public Role Role { get; set; }

        [Column("avatar")]
        public byte[]? Avatar { get; set; }

        public List<Vote> Votes { get; set; } = new();
    }
}
