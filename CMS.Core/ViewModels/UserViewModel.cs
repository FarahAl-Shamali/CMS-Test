using CMS.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace CMS.Core.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhineNumber { get; set; }
        public DateTime? DOB { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public UserType UserType { get; set; }
    }
}
