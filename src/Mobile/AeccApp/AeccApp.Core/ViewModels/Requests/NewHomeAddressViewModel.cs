using AeccApp.Core.Extensions;
using AeccApp.Core.Models;
using AeccApp.Core.Services;
using AeccApp.Core.Validations;
using AeccApp.Core.ViewModels.Popups;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AeccApp.Core.ViewModels
{
    public class NewHomeAddressViewModel : ViewModelBase
    {
        private IGoogleMapsPlaceService GoogleMapsPlaceService { get; } = ServiceLocator.GoogleMapsPlaceService;
       

        public NewHomeAddressViewModel()
        {
            SugestedAddressesList = new ObservableCollection<AddressModel>();
            RequestAskForAddressNumberPopupVM = new RequestAskForAddressNumberPopupViewModel();
            RequestThereIsNoResultsPopupVM = new RequestThereIsNoResultsPopupViewModel();
            AddressName.Validations.Add(new IsNotNullOrEmptyRule { ValidationMessage = this["NewHomeAddressViewMustNameTheAddress"] });
        }

        public override Task ActivateAsync()
        {
            RequestAskForAddressNumberPopupVM.ContinueWithoutInputANumber += OnDoesNotWantToInputNumberSuggestionPopupCommand;
            return null;
        }

        public override void Deactivate()
        {
            RequestAskForAddressNumberPopupVM.ContinueWithoutInputANumber -= OnDoesNotWantToInputNumberSuggestionPopupCommand;
        }

        #region Properties

        #region Visibility related properties

        private bool _isSearchIconVisible;
        public bool IsSearchIconVisible
        {
            get { return _isSearchIconVisible; }
            set { Set(ref _isSearchIconVisible, value); }
        }

        private bool _showHelpMessage = true;
        public bool ShowHelpMessage
        {
            get { return _showHelpMessage; }
            set { Set(ref _showHelpMessage, value); }
        }
        #endregion

        
        public ObservableCollection<AddressModel> SugestedAddressesList { get; private set; }

        private bool _sugestedAddressesListIsEmpty;
        public bool SugestedAddressesListIsEmpty
        {
            get { return _sugestedAddressesListIsEmpty; }
            set { Set(ref _sugestedAddressesListIsEmpty, value); }
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

        private string _addressFinder = string.Empty;
        public string AddressFinder
        {
            get { return _addressFinder; }
            set { Set(ref _addressFinder, value); }
        }

        public RequestAskForAddressNumberPopupViewModel RequestAskForAddressNumberPopupVM { get; private set; }
        public RequestThereIsNoResultsPopupViewModel RequestThereIsNoResultsPopupVM { get; private set; }

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

        private Command _addressSelectedCommand;
        public ICommand AddressSelectedCommand
        {
            get
            {
                return _addressSelectedCommand ??
                    (_addressSelectedCommand = new Command(OnAddressSelected, o => !IsBusy));
            }
        }

        private void OnAddressSelected(object obj)
        {
            // ListView ItemTapped event is called twice
            var newAddressSelected = obj as AddressModel;
            if (newAddressSelected != AddressSelected)
            {
                AddressSelected = newAddressSelected;
                AddressFinder = AddressSelected.FinderAddress;
            }
        }

        private Command _continueWithRequestCommand;
        public ICommand ContinueWithRequestCommand
        {
            get
            {
                return _continueWithRequestCommand ??
                    (_continueWithRequestCommand = new Command(o => OnContinueWithRequestAsync(), o => !IsBusy));
            }
        }

        private async Task OnContinueWithRequestAsync()
        {
            if (IsAddressGettingSaved && !AddressName.Validate())
                return;

            if (string.IsNullOrWhiteSpace(AddressSelected.Number))
            {
                await NavigationService.ShowPopupAsync(RequestAskForAddressNumberPopupVM);
            }
            else
            {
                await InternalContinueWithRequestAsync();
            }
        }

        private Command _resetAddressFinderCommand;
        public ICommand ResetAddressFinderCommand
        {
            get
            {
                return _resetAddressFinderCommand ??
                    (_resetAddressFinderCommand = new Command(o=> OnResetAddressFinder(), o => !IsBusy));
            }
        }

        private void OnResetAddressFinder()
        {
            AddressSelected = null;
            AddressFinder = string.Empty;

            SugestedAddressesListIsEmpty = false;
            ShowHelpMessage = true;
        }


        private Command _addressChangedCommand;
        public ICommand AddressChangedCommand
        {
            get
            {
                return _addressChangedCommand ??
                    (_addressChangedCommand = new Command(OnAddressChanged, o => !IsBusy));
            }
        }

        private void OnAddressChanged(object obj)
        {
            if (AddressSelected != null)
                return;

            string result = string.Empty;
            if (obj is string)
            {
                result = (string)obj;
            }

            if (string.IsNullOrWhiteSpace(result))
            {
                IsSearchIconVisible = false;
                ShowHelpMessage = true;
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
                ShowHelpMessage = false;
            }
        }

        private Command _addressGettingSaved;
        public ICommand AddressGettingSavedCommand
        {
            get
            {
                return _addressGettingSaved ??
                    (_addressGettingSaved = new Command(OnAddressGettingSaved, o => !IsBusy));
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
        private Task InternalContinueWithRequestAsync()
        {
            return ExecuteOperationAsync(async () =>
            {
                // Save new home address
                if (IsAddressGettingSaved)
                    AddressSelected.WillBeSaved = true;

                if (string.IsNullOrEmpty(AddressSelected.PlaceId))
                {
                    var places = await GoogleMapsPlaceService.FindPlacesAsync(AddressSelected.FinderAddress);
                    if (places.Any())
                    {
                        AddressSelected = places.First();
                        AddressSelected.Name = AddressName.Value;
                        
                        await NavigationService.NavigateToAsync<NewHomeRequestChooseTypeViewModel>(AddressSelected);
                        await NavigationService.RemoveLastFromBackStackAsync();
                    }
                    else
                    {
                        await NavigationService.ShowPopupAsync(RequestThereIsNoResultsPopupVM);
                    }
                }
                else
                {
                    AddressSelected.Name = AddressName.Value;
                    await NavigationService.NavigateToAsync<NewHomeRequestChooseTypeViewModel>(AddressSelected);
                    await NavigationService.RemoveLastFromBackStackAsync();
                }
            });
        }

        public async void OnDoesNotWantToInputNumberSuggestionPopupCommand(object sender, EventArgs e)
        {
            await NavigationService.HidePopupAsync();
            await InternalContinueWithRequestAsync();
        }

        public override bool OnBackButtonPressed()
        {
            bool returnValue = false;
            if (AddressSelected != null)
            {
                OnResetAddressFinder();
                returnValue = true;
            }

            return returnValue;
        }

        private Task RefreshSugestedAddressesAsync(string result)
        {
            return ExecuteOperationAsync(async () =>
            {
                if (SugestedAddressesList.Any())
                    SugestedAddressesList.Clear();

                var places = await GoogleMapsPlaceService.FindPlacesAsync(result);
                SugestedAddressesList.AddRange(places);

                SugestedAddressesListIsEmpty = !SugestedAddressesList.Any();
            });
        }

        protected override void OnIsBusyChanged()
        {
            _continueWithRequestCommand.ChangeCanExecute();
        }
        #endregion

    }
}