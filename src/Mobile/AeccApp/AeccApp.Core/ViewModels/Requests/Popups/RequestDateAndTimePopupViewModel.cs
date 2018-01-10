using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace AeccApp.Core.ViewModels.Popups
{
    public class RequestDateAndTimePopupViewModel : ClosablePopupViewModelBase
    {
        public event EventHandler ApplyDateAndTime;

        public RequestDateAndTimePopupViewModel()
        {
            var now = DateTime.Now.AddHours(2);

            _dateSelected = now;
            _timeSelected = new TimeSpan(now.Hour, 0, 0);
        }

        private Command _applyDateAndTimeCommand;
        public ICommand ApplyDateAndTimeCommand
        {
            get
            {
                return _applyDateAndTimeCommand ??
                    (_applyDateAndTimeCommand = new Command(o => ApplyDateAndTime?.Invoke(this, null)));
            }
        }

        private DateTime _dateSelected;

        public DateTime DateSelected
        {
            get { return _dateSelected; }
            set { Set(ref _dateSelected, value); }
        }

        private TimeSpan _timeSelected;

        public TimeSpan TimeSelected
        {
            get { return _timeSelected; }
            set { Set(ref _timeSelected, value); }
        }




    }
}
