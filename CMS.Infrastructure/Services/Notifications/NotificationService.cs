using CMS.Core.DTOs;
using FirebaseAdmin.Messaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace CMS.Infrastructure.Services.Notifications
{
    public class NotificationService : INotificationService
    {
        public async Task<bool> SendByFCM(string token, NotificationDto dto)
        {
            try
            {
                var notification = new Message()
                {
                    Data = new Dictionary<string, string>()
                    {
                          { "Title", dto.Title },
                          { "Body", dto.Body },
                          { "Action", dto.Action.ToString() },
                          { "ActionId", dto.ActionId },
                    },
                    //Fid = token
                    //older but i will try it for now
                    Token = token
                };
                await FirebaseMessaging.DefaultInstance.SendAsync(notification);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
