using CMS.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace CMS.Infrastructure.Services.Notifications
{
    public interface INotificationService
    {
        Task<bool> SendByFCM(string token, NotificationDto dto);
    }
}
