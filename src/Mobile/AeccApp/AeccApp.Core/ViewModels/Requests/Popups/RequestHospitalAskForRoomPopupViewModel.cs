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

        private Command _roomEventArgsCommand;
        public ICommand RoomEventArgsCommand
        {
            get
            {
                return _roomEventArgsCommand ??
                    (_roomEventArgsCommand = new Command(OnRoomEventArgsCommand, o => !IsBusy));
            }
        }

        private void OnRoomEventArgsCommand(object obj)
        {
            string result = string.Empty;
            if (obj is string)
            {
                result = (string)obj;
                if (result.Length <= 0)
                {
                    IsRoomFormFilled = false;
                }
                else
                {
                    IsRoomFormFilled = true;
                }
                if (Room.Length <= 40)
                {
                    MaxLenghtIndicator = result.Length + "/40";
                }
                else
                {
                    Room = Room.Remove(40);
                }
            }
        }

        #endregion


        #region Properties
        private bool _isRoomFormFilled;

        public bool IsRoomFormFilled
        {
            get { return _isRoomFormFilled; }
            set { Set(ref _isRoomFormFilled, value); }
        }



        private string _maxLenghtIndicator = "0/40";

        public string MaxLenghtIndicator
        {
            get { return _maxLenghtIndicator; }
            set { Set(ref _maxLenghtIndicator, value); }
        }


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
