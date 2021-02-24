using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IssueTracker.ViewModels
{
    public class UserVM
    {
        public string ApplicationUser  { get; set; }

        public string Email { get; set; }

        public string FullName { get; set; }

        public string AvatarPath { get; set; }

        public string LargeAvatarPath { get; set; }

        public string UserRole { get; set; }
    }
}