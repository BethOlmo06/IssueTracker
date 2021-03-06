﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IssueTracker.ViewModels
{
    public class ProjectWizardVM
    {
        [Required]
        public string Name { get; set; }

        public string ProjectManagerId { get; set; }

        public virtual ICollection<string> DeveloperIds { get; set; }

        public virtual ICollection<string> SubmitterIds { get; set; }

        public ProjectWizardVM()
        {
            DeveloperIds = new HashSet<string>();
            SubmitterIds = new HashSet<string>();
        }
    }
}