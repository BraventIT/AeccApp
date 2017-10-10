using System;
using AeccApp.Core.ViewModels;
using Xamarin.Forms;

namespace AeccApp.Core.Views
{
    public class BaseContentPage : ContentPage
    {
        protected INavigableViewModel ViewModel => BindingContext as INavigableViewModel;

        public BaseContentPage()
        {
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            if (ViewModel != null)
                ViewModel.ViewIsInitialized = true;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ViewModel?.ActivateAsync();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            if (ViewModel != null)
                ViewModel.ViewIsInitialized = false;
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
                if (Device.RuntimePlatform == Device.Windows)
                {
                    path = $"Assets/{path}.png";
                }

                source.File = path;
                SetValue(IconProperty, source);
            }
        }
    }
}
