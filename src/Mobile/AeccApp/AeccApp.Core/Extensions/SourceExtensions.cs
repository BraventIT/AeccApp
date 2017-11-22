using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AeccApp.Core.Extensions
{
    public static class SourceExtensions
    {
        public static void SetSourcePlatform(this Image image, string sourcePath)
        {
            image.Source = GetSourceFixed(sourcePath);
        }
        public static void SetSourcePlatform(this ToolbarItem image, string sourcePath)
        {
            image.Icon = GetSourceFixed(sourcePath);
        }

        public static void SetSourcePlatform(this ContentPage image, string sourcePath)
        {
            image.Icon = GetSourceFixed(sourcePath);
        }


        private static FileImageSource GetSourceFixed(string sourcePath)
        {
            var path =  GetPathFixed( sourcePath);
            return (FileImageSource)ImageSource.FromFile(path);
        }

        public static string GetPathFixed(string sourcePath)
        {
            var path = sourcePath;
            if (Device.RuntimePlatform == Device.UWP)
            {
                path = (path.EndsWith(".jpg", StringComparison.CurrentCultureIgnoreCase)) ?
                    $"Assets/{path}" : $"Assets/{path}.png";
            }
            return path;
        }
    }
}
