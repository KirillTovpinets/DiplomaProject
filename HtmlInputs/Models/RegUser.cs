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
        [RegularExpression("[0-9]{2}[a-z]{3}[0-4]{1}[a-z]{3,6}", ErrorMessage = "Неверный формат логина.")]
        public string Login { get; set; }
        [Required(ErrorMessage = "Введите пароль", AllowEmptyStrings = false)]
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
        public DateTime Birthday { get; set; }
        public int RoleId { get; set; }
        [Required(ErrorMessage = "Введите email", AllowEmptyStrings = false)]
        [EmailAddress(ErrorMessage = "Введены не правильные данные")]
        public string Email { get; set; }
        public string AvatarPath { get; set; }
        public int Group { get; set; }
        public int Course { get; set; }
    }
}
