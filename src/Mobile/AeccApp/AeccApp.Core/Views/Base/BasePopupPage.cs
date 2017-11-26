using AeccApp.Core.ViewModels;
using Rg.Plugins.Popup.Pages;
using System.Threading.Tasks;

namespace AeccApp.Core.Views.Popups
{
    public class BasePopupPage: PopupPage
    {
        protected INavigableViewModel ViewModel => BindingContext as INavigableViewModel;

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
    }
}
