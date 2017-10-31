using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace AeccApp.Core.ViewModels.Popups
{
   public class RequestDateAndTimePopupViewModel : ClosablePopupViewModelBase
    {
        public event EventHandler ApplyDateAndTime;

        private Command _applyDateAndTimeCommand;
        public ICommand ApplyDateAndTimeCommand
        {
            get
            {
                return _applyDateAndTimeCommand ??
                    (_applyDateAndTimeCommand = new Command(o => ApplyDateAndTime?.Invoke(this, null)));
            }
        }

        private DateTime _dateSelected = DateTime.Now;

        public DateTime DateSelected
        {
            get { return _dateSelected; }
            set { Set(ref _dateSelected, value); }
        }

        private TimeSpan _timeSelected = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

        public TimeSpan TimeSelected
        {
            get { return _timeSelected; }
            set { Set(ref _timeSelected, value); }
        }




    }
}
