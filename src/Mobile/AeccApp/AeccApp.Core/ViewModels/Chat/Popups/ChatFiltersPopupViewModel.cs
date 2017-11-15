using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace AeccApp.Core.ViewModels.Popups
{
    public class ChatFiltersPopupViewModel : ViewModelBase
    {
        private const int MIN_AGE = 18;
        private const int MAX_AGE = 80;

        /// <summary>
        /// Raised when filters are applied
        /// </summary>
        public event EventHandler AppliedFilters;

        public ChatFiltersPopupViewModel()
        {
            Reset();
        }

        private int _minimumAge;
        public int MinimumAge
        {
            get { return _minimumAge; }
            set { Set(ref _minimumAge, value); }
        }

        private int _maximumAge;
        public int MaximumAge
        {
            get { return _maximumAge; }
            set { Set(ref _maximumAge, value); }
        }



        public string Gender { get; set; }


        private bool _genderMen;

        public bool GenderMen
        {

            get { return Gender == null ? false : Gender.StartsWith("h", StringComparison.CurrentCultureIgnoreCase); }
            set { Set(ref _genderMen, value); }
        }
    

        private bool _genderWomen;

        public bool GenderWomen
        {
            get { return Gender == null ? false : Gender.StartsWith("m", StringComparison.CurrentCultureIgnoreCase); }
            set { Set(ref _genderWomen, value); }

        }


        private Command _switchGenderCommand;
        public ICommand SwitchGenderCommand
        {
            get
            {
                return _switchGenderCommand ??
                    (_switchGenderCommand = new Command(OnSwitchGenderCommand, o => !IsBusy));
            }
        }

        void OnSwitchGenderCommand(object obj)
        {
            switch (obj)
            {
                case "0"://female
                    if(Gender.StartsWith("m", StringComparison.CurrentCultureIgnoreCase))
                    {
                        Gender = "default";
                    }
                    else
                    {
                    Gender = "m";
                    }

                    NotifyPropertyChanged(nameof(GenderMen));
                    NotifyPropertyChanged(nameof(GenderWomen));

                    break;

                case "1"://male
                    if (Gender.StartsWith("h", StringComparison.CurrentCultureIgnoreCase))
                    {
                        Gender = "default";
                    }
                    else
                    {
                        Gender = "h";
                    }
                    NotifyPropertyChanged(nameof(GenderMen));
                    NotifyPropertyChanged(nameof(GenderWomen));
                    break;

            }

        }




        private Command _applyFiltersCommand;
        public ICommand ApplyFiltersCommand
        {
            get
            {
                return _applyFiltersCommand ??
                    (_applyFiltersCommand = new Command(o => AppliedFilters?.Invoke(this, null)));
            }
        }

        public void Reset()
        {
            Gender = "default";
            MinimumAge = MIN_AGE;
            MaximumAge = MAX_AGE;
        }
    }
}
