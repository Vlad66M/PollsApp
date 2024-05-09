using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PollsApp.Application.DTOs
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Не указан email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Не указан пароль ")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        /*[DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароль введен неверно ")]
        public string ConfirmPassword { get; set; }*/

    }
}
