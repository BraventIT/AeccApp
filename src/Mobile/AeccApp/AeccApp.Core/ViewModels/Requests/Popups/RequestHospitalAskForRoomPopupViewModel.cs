using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace AeccApp.Core.ViewModels.Popups
{
    public class RequestHospitalAskForRoomPopupViewModel : ViewModelBase
    {

        #region Commands
        private Command _closePopupCommand;
        public ICommand ClosePopupCommand
        {
            get
            {
                return _closePopupCommand ??
                    (_closePopupCommand = new Command(OnClosePopupCommand, (o) => !IsBusy));
            }
        }

        public void OnClosePopupCommand(object obj)
        {
            NavigationService.HidePopupAsync();
        }

        private Command _continueWithRequestCommand;
        public ICommand ContinueWithRequestCommand
        {
            get
            {
                return _continueWithRequestCommand ??
                    (_continueWithRequestCommand = new Command(OnContinueWithRequestCommand, (o) => !IsBusy));
            }
        }

        public void OnContinueWithRequestCommand(object obj)
        {
                //TODO NAVIGATE TO NEXT PAGE WITH THE ROOM AND HALL STRING ADDED
        }


        #endregion


        #region Properties
        private string _room;
        public string Room
        {
            get { return _room; }
            set { Set(ref _room, value); }
        }

        private string _hall;
        public string Hall
        {
            get { return _hall; }
            set { Set(ref _hall, value); }
        }

        #endregion


    }
}
