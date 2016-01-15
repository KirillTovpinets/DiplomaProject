using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace HtmlInputs.Models
{
    public class LogedUser
    {
        [Required(ErrorMessage = "Введите логин", AllowEmptyStrings = false)]
        public string Login { get; set; }
        public string Password { get; set; }
    }
}