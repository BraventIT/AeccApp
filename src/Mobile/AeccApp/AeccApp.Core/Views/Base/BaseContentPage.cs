using System;
using AeccApp.Core.ViewModels;
using Xamarin.Forms;
using System.Threading.Tasks;
using AeccApp.Core.Extensions;
using System.Resources;
using System.Diagnostics;
using AeccApp.Internationalization.Properties;

namespace AeccApp.Core.Views
{
    public class BaseContentPage : ContentPage
    {
        protected INavigableViewModel ViewModel => BindingContext as INavigableViewModel;


        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await Task.Delay(200);
            ViewModelBase.UpdateToken();
            await ViewModel?.ActivateAsync();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            ViewModelBase.TryCancelToken();
            ViewModel?.Deactivate();
        }

        protected override bool OnBackButtonPressed()
        {
            return ViewModel?.OnBackButtonPressed() ?? base.OnBackButtonPressed();
        }

        public string IconPlatform
        {
            get { return (string)GetValue(IconPlatformProperty); }
            set { SetValue(IconPlatformProperty, value); }
        }

        public static BindableProperty IconPlatformProperty = BindableProperty.Create(
            nameof(IconPlatform),
            typeof(string),
            typeof(BaseContentPage),
            null,
            propertyChanged: OnSourcePlatformChanged);

        private static void OnSourcePlatformChanged(BindableObject b, object oldValue, object newValue)
        {
            if (newValue == null)
                return;
            var control = (BaseContentPage)b;
            control.SetSourcePlatform((string)newValue);
        }


        #region Localization Resources
        protected virtual ResourceManager LocalizationResourceManager { get; } = LocalizationResourcesAecc.ResourceManager;

        /// <summary>
		/// Obtiene el texto desde recursos para la key pasada (index).
		/// </summary>
		/// <param name="index">La "KEY" del texto de recursos que queramos obtener.</param>
		/// <returns>El texto traducido a la cultura establecida.</returns>
		public string this[string index]
        {
            get
            {
                try
                {
                    return LocalizationResourceManager.GetString(index) ?? $"_{index}";
                }
                catch (Exception)
                {
                    if (Debugger.IsAttached)
                    {
                        Debugger.Break();
                    }

                    return $"_{index}";
                }

            }
        }
        #endregion


    }
}
