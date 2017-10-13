using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace AeccApp.Core.ViewModels.Popups
{
    public class ChatFiltersPopupViewModel: ViewModelBase
    {
        /// <summary>
        /// Raised when filters are applied
        /// </summary>
        public event EventHandler AppliedFilters;

        private int _minimumAge = 18;
        public int MinimumAge
        {
            get { return _minimumAge; }
            set { Set(ref _minimumAge, value); }
        }

        private int _maximumAge = 80;
        public int MaximumAge
        {
            get { return _maximumAge; }
            set { Set(ref _maximumAge, value); }
        }

        public enum Gender
        {
            Male,
            Female
        }

        private Command _applyFiltersCommand;
        public ICommand ApplyFiltersCommand
        {
            get
            {
                return _applyFiltersCommand ??
                    (_applyFiltersCommand = new Command(o=> AppliedFilters?.Invoke(this, null)));
            }
        }
    }
}
