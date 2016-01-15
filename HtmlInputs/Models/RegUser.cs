using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace HtmlInputs.Models
{
    public class RegUser
    {
        public int UserId { get; set; }
        [Required(ErrorMessage="Введите логин",AllowEmptyStrings=false)]
        public string Login { get; set; }
        [Required(ErrorMessage = "Введите логин", AllowEmptyStrings = false)]
        public string Password { get; set; }
        [Compare("Password",ErrorMessage="Пароли не совпадают")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "Введите число с картинки", AllowEmptyStrings = false)]
        public string Captcha { get; set; }
        [Required(ErrorMessage = "Введите имя", AllowEmptyStrings = false)]
        public string Name { get; set; }
        [Required(ErrorMessage = "Введите фамилию", AllowEmptyStrings = false)]
        public string Sirname { get; set; }
        [Required(ErrorMessage = "Введите отчество", AllowEmptyStrings = false)]
        public string Patername { get; set; }
        public int DayOfBirth { get; set; }
        public int MonthOfBirth { get; set; }
        public int YearOfBirth { get; set; }
        public int RoleId { get; set; }
        [Required(ErrorMessage = "Введите email", AllowEmptyStrings = false)]
        public string Email { get; set; }
        public string AvatarPath { get; set; }
        public int Group { get; set; }
        public int Course { get; set; }
    }
}
