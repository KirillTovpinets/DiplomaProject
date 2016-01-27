using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace HtmlInputs.Models
{
    public class AddMiss
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        [Required(ErrorMessage = "Введите форму занятий", AllowEmptyStrings = false)]
        public string Form { get; set; }
        [Required(ErrorMessage = "Введите название дисциплины", AllowEmptyStrings = false)]
        public string Discipline { get; set; }
        public int IsValid { get; set; }
        public int IsRead { get; set; }
        public DateTime Date { get; set; }
        public virtual Users Users { get; set; }
    }
}