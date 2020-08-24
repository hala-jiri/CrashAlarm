using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Plugin.Permissions;
using Xamarin.Essentials;

namespace CrashAlarm
{
    public class PermissionService
    {
        public async Task<PermissionStatus> CheckAndRequestSMSPermission()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.Sms>();
            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.Sms>();
            }

            // Additionally could prompt the user to turn on in settings
            Console.WriteLine($"Permission: {status}");
            return (PermissionStatus)status;
        }
    }
}
