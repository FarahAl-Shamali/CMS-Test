using CMS.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace CMS.Core.DTOs
{
    public class NotificationDto
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public NotificationAction Action { get; set; }
        public string ActionId { get; set; }
    }
}
