using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Project.Models
{
    public class MailModel
    {
        public string To { get; set; }
        public string Subject { get; set; }
        [DataType(DataType.MultilineText)]
        public string Body { get; set; }
    }
}