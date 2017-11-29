using AeccApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace AeccApp.Core.ViewModels
{
    public class ChatFiltersViewModel : ViewModelBase
    {

        private const int MIN_AGE = 18;
        private const int MAX_AGE = 80;


        public override Task InitializeAsync(object navigationData)
        {
            ChatFiltersModel Filters;
            if (navigationData != null)
            {
                Filters = navigationData as ChatFiltersModel;
                MinimumAge = Filters.MinimumAge;
                MaximumAge = Filters.MaximumAge;
                Gender = Filters.Gender;
            }         

            return Task.CompletedTask;

        }


        public ChatFiltersViewModel()
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



        private string _gender = string.Empty;
        public string Gender
        {
            get { return _gender; }
            set
            {
                if (Set(ref _gender, value))
                {
                    NotifyPropertyChanged(nameof(GenderWomen));
                    NotifyPropertyChanged(nameof(GenderMen));
                }
            }
        }

        public bool GenderMen
        {
            get { return Gender.StartsWith("h", StringComparison.CurrentCultureIgnoreCase); }
        }

        public bool GenderWomen
        {
            get { return Gender.StartsWith("m", StringComparison.CurrentCultureIgnoreCase); }
        }

        private Command _switchGenderCommand;
        public ICommand SwitchGenderCommand
        {
            get
            {
                return _switchGenderCommand ??
                    (_switchGenderCommand = new Command(OnSwitchGenderCommand));
            }
        }

        void OnSwitchGenderCommand(object obj)
        {
            string selectedGender = (string)obj;

            Gender = (Gender.StartsWith(selectedGender, StringComparison.CurrentCultureIgnoreCase)) ?
                      string.Empty : selectedGender;
        }

        private Command _applyFiltersCommand;
        public ICommand ApplyFiltersCommand
        {
            get
            {
                return _applyFiltersCommand ??
                    (_applyFiltersCommand = new Command(OnApplyFilters));
            }
        }

        async void OnApplyFilters()
        {
            await NavigationService.NavigateBackAsync();
        }

        private Command _resetFiltersCommand;
        public ICommand ResetFiltersCommand
        {
            get
            {
                return _resetFiltersCommand ??
                    (_resetFiltersCommand = new Command(OnResetFilters));
            }
        }

        void OnResetFilters()
        {
            Reset();
        }

        public void Reset()
        {
            Gender = string.Empty;
            MinimumAge = MIN_AGE;
            MaximumAge = MAX_AGE;
        }


    }
}

