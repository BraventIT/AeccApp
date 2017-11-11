using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace AeccApp.Core.ViewModels.Popups
{
    public class ChatRatingPopupViewModel : ViewModelBase
    {

        private Command _ratingCommand;
        public ICommand RatingCommand
        {
            get
            {
                return _ratingCommand ??
                    (_ratingCommand = new Command(OnRatingCommand, o => !IsBusy));
            }
        }

        private async void OnRatingCommand(object obj)
        {
            //TODO #18 Send rating with email service 
            switch (obj)
            {
                case "1":
                    var test1 = "1 estrella";
                    break;

                case "2":
                    var test2 = "2 estrellas";

                    break;

                case "3":
                    var test3 = "3 estrellas";

                    break;

                case "4":
                    var test4 = "4 estrellas";

                    break;

                case "5":
                    var test = "5 estrellas";

                    break;

                default:
                    break;
            }
            await NavigationService.HidePopupAsync();
            await ExecuteOperationAsync(() => NavigationService.NavigateToAsync<DashboardViewModel>());
        }

        public override bool OnBackButtonPressed()
        {
            return true;
        }

        }
}
