using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AeccApp.Core.Extensions
{
    public static class PermissionsExtension
    {
        public static async Task<bool> CheckPermissionsAsync(this IPermissions permissionsService, Permission permission)
        {
            var permissionStatus = await permissionsService.CheckPermissionStatusAsync(permission);
            bool request = false;
            if (permissionStatus == PermissionStatus.Denied)
            {
                if (Device.RuntimePlatform == Device.iOS)
                {

                    var title = App.Current.Resources["LocationPermissionsTitle"] + $"{permission}";
                    var question = (string)App.Current.Resources["LocationPermissionsQuestion"] + $"{permission}";
                    var positive = (string)App.Current.Resources["LocationPermissionsPositive"];
                    var negative = (string)App.Current.Resources["LocationPermissionsNegative"];
                    var task = Application.Current?.MainPage?.DisplayAlert(title, question, positive, negative);
                    if (task == null)
                        return false;

                    var result = await task;
                    if (result)
                    {
                        permissionsService.OpenAppSettings();
                    }

                    return false;
                }

                request = true;

            }

            if (request || permissionStatus != PermissionStatus.Granted)
            {
                var newStatus = await permissionsService.RequestPermissionsAsync(permission);
                if (newStatus.ContainsKey(permission) && newStatus[permission] != PermissionStatus.Granted)
                {
                    var title = App.Current.Resources["LocationPermissionsTitle"] + $"{permission}";
                    var question = (string)App.Current.Resources["LocationPermissionsQuestion"] + $"{permission}";
                    var positive = (string)App.Current.Resources["LocationPermissionsPositive"];
                    var negative = (string)App.Current.Resources["LocationPermissionsNegative"];
                    var task = Application.Current?.MainPage?.DisplayAlert(title, question, positive, negative);
                    if (task == null)
                        return false;

                    var result = await task;
                    if (result)
                    {
                        permissionsService.OpenAppSettings();
                    }
                    return false;
                }
            }

            return true;
        }
    }
}
