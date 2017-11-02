using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AeccApp.Core.ViewModels.Popups
{
    public class ClosablePopupViewModelBase : ViewModelBase
    {
        /// <summary>
        /// Closes logout popup
        /// </summary>
        private Command _closePopupCommand;
        public ICommand ClosePopupCommand
        {
            get
            {
                return _closePopupCommand ??
                    (_closePopupCommand = new Command(o=> OnClosePopupCommandAsync()));
            }
        }
        protected virtual Task OnClosePopupCommandAsync()
        {
            return NavigationService.HidePopupAsync();
        }
    }
}
