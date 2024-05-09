using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PollsApp.Application.DTOs
{
    public class CreatePollViewModel
    {
        [Required(ErrorMessage = "Не указан заголовок опроса")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Не добалены опции опроса")]
        public List<string> Options { get; set; }
        public bool HasEndDate { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; }
        public bool AllowComments { get; set; }

    }
}
