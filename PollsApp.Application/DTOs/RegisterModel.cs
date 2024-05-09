using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace PollsApp.Application.DTOs
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Не указан email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Не указан пароль ")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароль введен неверно ")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Не указано имя")]
        public string Name { get; set; }

        public byte[]? Avatar { get; set; }

        public string? CroppedAvatarBase64String { get; set; }
    }
}
