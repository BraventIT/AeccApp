using AeccApp.Core.Models;
using AeccApp.Core.Services;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AeccApp.Core.ViewModels.Popups
{
    public class ChatRatingPopupViewModel : ClosablePopupViewModelBase
    {
        private const int MIN_RATING = 3;

        private IChatService ChatService { get; } = ServiceLocator.ChatService;
        private IEmailService EmailService { get; } = ServiceLocator.EmailService;

        private UserData _counterpart;

        public ChatRatingPopupViewModel(UserData counterpart)
        {
            _counterpart = counterpart;
        }

        #region Commands
        private Command<int> _ratingCommand;
        public ICommand RatingCommand
        {
            get
            {
                return _ratingCommand ??
                    (_ratingCommand = new Command<int>((rating) => OnRatingAsync(rating).ConfigureAwait(false), (o) => !IsBusy));
            }
        }

        private Task OnRatingAsync(int rating)
        {
            return ExecuteOperationAsync(
                executeAction: async cancelToken =>
                {
                    if (rating <= MIN_RATING)
                    {
                        var emailFromChat = new EmailFromChat(
                            _counterpart,
                            ChatService.GetConversationMessages().Reverse(),
                            rating);
                        await EmailService.SendAsync(emailFromChat, cancelToken);
                    }
                },
                finallyAction: async () =>
                {
                    await NavigationService.HidePopupAsync();
                    await NavigationService.NavigateToAsync<DashboardViewModel>();
                });
        }
        #endregion

        #region Private Methods
        protected override void OnIsBusyChanged()
        {
            _ratingCommand.ChangeCanExecute();
        }

        #endregion
    }
}
