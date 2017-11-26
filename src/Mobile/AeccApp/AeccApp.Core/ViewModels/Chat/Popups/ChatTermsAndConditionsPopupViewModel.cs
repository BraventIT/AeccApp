using AeccApp.Core.Messages;
using System.Windows.Input;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace AeccApp.Core.ViewModels.Popups
{
    public class ChatTermsAndConditionsPopupViewModel : ClosablePopupViewModelBase
    {
        private bool _acceptTerms = false;

        public bool AcceptTerms
        {
            get { return _acceptTerms; }
            set
            {
                if (Set(ref _acceptTerms, value))
                {
                    _acceptTermsCommand?.ChangeCanExecute();
                }
            }
        }

        private Command _acceptTermsCommand;
        public ICommand AcceptTermsCommand
        {
            get
            {
                return _acceptTermsCommand ??
                    (_acceptTermsCommand = new Command(OnAcceptTermsCommand, o => AcceptTerms));
            }
        }

        private async void OnAcceptTermsCommand(object obj)
        {
            Settings.TermsAndConditionsAccept = true;
            await NavigationService.HidePopupAsync();
            //TODO REFRESH CHAT TAB

        }


        protected override async Task OnClosePopupCommandAsync()
        {
            await NavigationService.HidePopupAsync();
            MessagingCenter.Send(new DashboardTabMessage(TabsEnum.Home), string.Empty);
        }
    }
}
