﻿using AeccApp.Core.Extensions;
using AeccApp.Core.Models;
using AeccApp.Core.Services;
using AeccApp.Core.Validations;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AeccApp.Core.ViewModels
{
    public class NewHomeAddressViewModel : ViewModelBase
    {
        private readonly IGoogleMapsPlaceService _googleMapsPlaceService;

        public NewHomeAddressViewModel()
        {
            _googleMapsPlaceService= ServiceLocator.Resolve<IGoogleMapsPlaceService>();

            AddressName.Validations.Add(new IsNotNullOrEmptyRule { ValidationMessage = this["NewHomeAddressViewMustNameTheAddress"] });
        }

        #region Properties

        #region Visibility related properties
        private bool _isNumberSuggestionPopupVisible;

        public bool IsNumberSuggestionPopupVisible
        {
            get { return _isNumberSuggestionPopupVisible; }
            set { Set(ref _isNumberSuggestionPopupVisible, value); }
        }

        private bool _isSearchIconVisible;

        public bool IsSearchIconVisible
        {
            get { return _isSearchIconVisible; }
            set { Set(ref _isSearchIconVisible, value); }
        }

        #endregion

        private ObservableCollection<AddressModel> _sugestedAddressesList = new ObservableCollection<AddressModel>();
        public ObservableCollection<AddressModel> SugestedAddressesList
        {
            get { return _sugestedAddressesList; }
            set { Set(ref _sugestedAddressesList, value); }
        }

        private AddressModel _addressSelected;
        public AddressModel AddressSelected
        {
            get { return _addressSelected; }
            set
            {
                if (Set(ref _addressSelected, value))
                    NotifyPropertyChanged(nameof(AddressNumber));
            }
        }

        private string _address = string.Empty;
        public string Address
        {
            get { return _address; }
            set { Set(ref _address, value); }
        }

        #region Address related properties

        private ValidatableObject<string> _addressName = new ValidatableObject<string>();

        public ValidatableObject<string> AddressName
        {
            get { return _addressName; }
            set { Set(ref _addressName, value); }
        }

        public string AddressNumber
        {
            get { return AddressSelected?.Number; }
            set
            {
                AddressSelected.Number = value;
                AddressSelected.PlaceId = null;
            }
        }

        private bool _isAddressGettingSaved;

        public bool IsAddressGettingSaved
        {
            get { return _isAddressGettingSaved; }
            set { Set(ref _isAddressGettingSaved, value); }
        }

        #endregion
        #endregion

        #region Commands

        private Command _closeNumberSuggestionPopupCommand;
        public ICommand CloseNumberSuggestionPopupCommand
        {
            get
            {
                return _closeNumberSuggestionPopupCommand ??
                    (_closeNumberSuggestionPopupCommand = new Command(OnCloseNumberSuggestionPopupCommand, (o) => !IsBusy));
            }
        }

        private void OnCloseNumberSuggestionPopupCommand(object obj)
        {
            IsNumberSuggestionPopupVisible = false;
        }

        private Command _doesNotWantToInputNumberSuggestionPopupCommand;
        public ICommand DoesNotWantToInputNumberSuggestionPopupCommand
        {
            get
            {
                return _doesNotWantToInputNumberSuggestionPopupCommand ??
                    (_doesNotWantToInputNumberSuggestionPopupCommand = new Command(o => InternalContinueWithRequestAsync(), (o) => !IsBusy));
            }
        }

        private Command _addressSelectedCommand;
        public ICommand AddressSelectedCommand
        {
            get
            {
                return _addressSelectedCommand ??
                    (_addressSelectedCommand = new Command(OnAddressSelected, (o) => !IsBusy));
            }
        }

        private void OnAddressSelected(object obj)
        {
            AddressSelected = obj as AddressModel;
            Address = AddressSelected.DisplayAddress;
        }

        private Command _continueWithRequestCommand;
        public ICommand ContinueWithRequestCommand
        {
            get
            {
                return _continueWithRequestCommand ??
                    (_continueWithRequestCommand = new Command(o => OnContinueWithRequestAsync(), (o) => !IsBusy));
            }
        }

        private async Task OnContinueWithRequestAsync()
        {
            if (IsAddressGettingSaved && !AddressName.Validate())
                return;

            if (string.IsNullOrWhiteSpace(AddressSelected.Number))
            {
                IsNumberSuggestionPopupVisible = true;
            }
            else
            {
                await InternalContinueWithRequestAsync();
            }
        }

        private Task InternalContinueWithRequestAsync()
        {
            return ExecuteOperationAsync(async () =>
            {
                AddressSelected.Name = AddressName.Value;

                if (string.IsNullOrEmpty(AddressSelected.PlaceId))
                {
                    var places = await _googleMapsPlaceService.FindPlacesAsync(AddressSelected.DisplayAddress);
                    if (places.Any())
                    {
                        AddressSelected = places.First();
                    }
                }

                // Save new home address
                if (IsAddressGettingSaved)
                {
                    // TODO Save address
                }

                await NavigationService.NavigateToAsync<NewHomeRequestChooseTypeViewModel>(AddressSelected);
                await NavigationService.RemoveLastFromBackStackAsync();
            });
        }

        private Command _resetAddressCommand;
        public ICommand ResetAddressCommand
        {
            get
            {
                return _resetAddressCommand ??
                    (_resetAddressCommand = new Command(OnResetAddress, (o) => !IsBusy));
            }
        }

        private void OnResetAddress(object obj)
        {
            SugestedAddressesList.Clear();
            Address = string.Empty;
            AddressSelected = null;
        }


        private Command _addressChangedCommand;
        public ICommand AddressChangedCommand
        {
            get
            {
                return _addressChangedCommand ??
                    (_addressChangedCommand = new Command(OnAddressChanged, (o) => !IsBusy));
            }
        }

        private void OnAddressChanged(object obj)
        {
            string result = string.Empty;
            if (obj is string)
            {
                result = (string)obj;
            }

            if (string.IsNullOrWhiteSpace(result))
            {
                IsSearchIconVisible = false;
            }
            else
            {
                //Address suggestions with 5 characters or more
                if (result.Length > 4)
                {
                    RefreshSugestedAddressesAsync(result);
                }
                else if (SugestedAddressesList.Any())
                {
                    SugestedAddressesList.Clear();
                }
                IsSearchIconVisible = true;
            }
        }


        private Command _addressGettingSaved;
        public ICommand AddressGettingSavedCommand
        {
            get
            {
                return _addressGettingSaved ??
                    (_addressGettingSaved = new Command(OnAddressGettingSaved, (o) => !IsBusy));
            }
        }

        private void OnAddressGettingSaved(object obj)
        {
            bool result = false;
            if (obj is bool)
                result = (bool)obj;

            IsAddressGettingSaved = result;
        }
        #endregion

        #region Methods

        public override bool OnBackButtonPressed()
        {
            bool returnValue = false;
            if (IsNumberSuggestionPopupVisible)
            {
                IsNumberSuggestionPopupVisible = false;
                returnValue = true;
            }
            if (AddressSelected != null)
            {
                AddressSelected = null;
                Address = string.Empty;
                returnValue = true;
            }

            return returnValue;
        }

        public Task RefreshSugestedAddressesAsync(string result)
        {
            return ExecuteOperationAsync(async () =>
            {
                SugestedAddressesList.Clear();
                var places = await _googleMapsPlaceService.FindPlacesAsync(result);
                SugestedAddressesList.AddRange(places);
            });
        }

        protected override void OnIsBusyChanged()
        {
            _continueWithRequestCommand.ChangeCanExecute();
        }
        #endregion

    }
}
