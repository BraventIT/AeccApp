using System;
using AeccApp.Core.ViewModels;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace AeccApp.Core.Views
{
    public class BaseContentPage : ContentPage
    {
        protected INavigableViewModel ViewModel => BindingContext as INavigableViewModel;

      
        //protected override void OnSizeAllocated(double width, double height)
        //{
        //    base.OnSizeAllocated(width, height);
        //}

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await Task.Delay(200);
            ViewModel?.ActivateAsync();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            ViewModel?.Deactivate();
        }

        protected override bool OnBackButtonPressed()
        {
            return ViewModel?.OnBackButtonPressed() ?? base.OnBackButtonPressed();
        }

        public ImageSource IconPlatform
        {
            get
            {
                return (ImageSource)GetValue(IconProperty);
            }
            set
            {
                if (value == null)
                {
                    return;
                }

                var source = (FileImageSource)value;

                string path = source.File;
                if (Device.RuntimePlatform == Device.UWP)
                {
                    path = $"Assets/{path}.png";
                }

                source.File = path;
                SetValue(IconProperty, source);
            }
        }
    }
}
