using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CMS.Core.ViewModels
{
    public class AdvertismentViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string ImageUrl { get; set; }

        public string WebsiteUrl { get; set; }
        public String StartDate { get; set; }
        public String EndDate { get; set; }
        public decimal Price { get; set; }
        public UserViewModel Owner { get; set; }
    }
}
