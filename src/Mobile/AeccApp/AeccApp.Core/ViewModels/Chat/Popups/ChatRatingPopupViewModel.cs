using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AeccApp.Core.Models.Email;
using AeccApp.Core.Services;
using Xamarin.Forms;

namespace AeccApp.Core.ViewModels.Popups
{
    public class ChatRatingPopupViewModel : ViewModelBase
    {
        private readonly IChatService _chatService;
        private readonly IEmailService _emailService;


        #region Contructor & Initialize

        public ChatRatingPopupViewModel()
        {
            _chatService = ServiceLocator.ChatService;
            _emailService = ServiceLocator.EmailService;
        }

        #endregion

        #region Commands
        private Command<int> _ratingCommand;
        public ICommand RatingCommand
        {
            get
            {
                return _ratingCommand ??
                    (_ratingCommand = new Command<int>((rating) => OnRatingCommand(rating).ConfigureAwait(false), (o) => !IsBusy));
            }
        }

        #endregion

        #region Public methods

        public override bool OnBackButtonPressed()
        {
            return true;
        }

        #endregion


        #region Private Methods

        private Task OnRatingCommand(int rating)
        {
            return ExecuteOperationAsync(
                executeAction: async cancelToken =>
                {
                    if (rating <= 3)
                    {
                        var emailFromChat = new EmailFromChat(_chatService.ConversationCounterpart,
                            _chatService.GetConversationMessages(), rating);
                        await _emailService.SendAsync(emailFromChat, cancelToken);
                    }
                },
                finallyAction: async () =>
                {
                    await NavigationService.HidePopupAsync();
                    await ExecuteOperationAsync(() => NavigationService.NavigateToAsync<DashboardViewModel>());
                });
        }
    }

        #endregion
    
}
