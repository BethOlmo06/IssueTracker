﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BethOlmo_blog.Models
{
    public class EmailModel
    {

        [Display(Name = "FromName")]
        public string FromName { get; set; }

        [Display(Name = "Email"), EmailAddress]
        public string FromEmail { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        public string Body { get; set; }
    }
}