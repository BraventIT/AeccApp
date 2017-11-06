using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace AeccApp.Core.ViewModels.Popups
{
    public class RequestHospitalAskForRoomPopupViewModel : ClosablePopupViewModelBase
    {
        public event EventHandler ContinueWithRequest;


        #region Commands
        private Command _continueWithRequestCommand;
        public ICommand ContinueWithRequestCommand
        {
            get
            {
                return _continueWithRequestCommand ??
                    (_continueWithRequestCommand = new Command(o => ContinueWithRequest?.Invoke(this, null)));
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
