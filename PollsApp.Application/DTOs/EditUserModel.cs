using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PollsApp.Application.DTOs
{
    public class EditUserModel
    {
        public string? Id {  get; set; }

        [Required(ErrorMessage = "Не указан email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Не указано имя")]
        public string Name { get; set; }

        public byte[]? Avatar { get; set; }

        public string? CroppedAvatarBase64String { get; set; }
    }
}
