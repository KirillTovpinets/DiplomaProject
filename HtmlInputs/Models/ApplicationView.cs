using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HtmlInputs.Models
{
    public class ApplicationView
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public string Reason { get; set; }
        public int isMale { get; set; }
        public DateTime DateOfMiss { get; set; }
        public System.DateTime DateOfCreation { get; set; }
        public virtual Users Users { get; set; }
    }
}