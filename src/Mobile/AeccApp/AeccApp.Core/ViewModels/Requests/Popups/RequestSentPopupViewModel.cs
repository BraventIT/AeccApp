using System.Threading.Tasks;

namespace AeccApp.Core.ViewModels.Popups
{
    public class RequestSentPopupViewModel : ClosablePopupViewModelBase
    {
        protected override async Task OnClosePopupCommandAsync()
        {
            await NavigationService.HidePopupAsync();
            await NavigationService.NavigateToAsync<DashboardViewModel>();
            await NavigationService.RemoveBackStackAsync();
        }

        public override bool OnBackButtonPressed()
        {
            OnClosePopupCommandAsync();
            return false;
        }
    }
}
