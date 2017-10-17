using AeccApp.Core.Extensions;
using AeccApp.Core.Models;
using AeccApp.Core.Services;
using AeccApp.Core.Validations;
using AeccApp.Core.ViewModels.Popups;
using AeccApp.Internationalization.Properties;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Resources;
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
            RequestAskForAddressNumberPopupVM = new RequestAskForAddressNumberPopupViewModel();
            _googleMapsPlaceService = ServiceLocator.Resolve<IGoogleMapsPlaceService>();       
            AddressName.Validations.Add(new IsNotNullOrEmptyRule { ValidationMessage = this["Debes nombrar esta dirección para guardarla"] });
        }

        public override Task ActivateAsync()
        {
            RequestAskForAddressNumberPopupVM.OnContinueWithoutInput += OnDoesNotWantToInputNumberSuggestionPopupCommand;
            return null;
        }

        public override void Deactivate()
        {
            RequestAskForAddressNumberPopupVM.OnContinueWithoutInput -= OnDoesNotWantToInputNumberSuggestionPopupCommand;

        }

        #region Commands

        private async void OnDoesNotWantToInputNumberSuggestionPopupCommand(object sender, EventArgs e)
        {
            if (IsAddressGettingSaved)
            {
                // Save new home address (without address number)
                await NavigationService.NavigateToAsync<NewHomeRequestChooseTypeViewModel>(NewAddress);
            }
            else
            {
                // pass home address without saving it (without address number)
                await NavigationService.NavigateToAsync<NewHomeRequestChooseTypeViewModel>(NewAddress);

            }
            await NavigationService.HidePopupAsync();
        }


        private Command _sugestionListTapped;
        public ICommand SugestionListTapped
        {
            get
            {
                return _sugestionListTapped ??
                    (_sugestionListTapped = new Command(OnSugestionListTapped, (o) => !IsBusy));
            }
        }

        private void OnSugestionListTapped(object obj)
        {
            AddressSelected = obj as AddressModel;
            Address = AddressSelected.DisplayAddress;
            NewAddress.Street = AddressSelected.DisplayAddress;
            NewAddress.PlaceId = AddressSelected.PlaceId;
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

        async private void OnContinueWithRequestCommand(object obj)
        {
            if (AddressName == null || AddressName.Validate() == false)
            {
                //Validate() doing its work
            }
            else
            {
                NewAddress.Name = AddressName.Value;
                NewAddress.Number = AddressSelected.DisplayAddress;
               // NewAddress.DisplayAddress = AddressSelected.DisplayAddress;

                if (!AddressNumberInfo.Equals(string.Empty))
                {
                    ThereIsAddressNumberOrDoesNotWantTo = true;
                    NewAddress.Number = "Nº" + AddressNumberInfo;

                    if (!AddressPortalInfo.Equals(string.Empty))
                    {
                        NewAddress.Number = NewAddress.Number + " Portal " + AddressPortalInfo;

                    }

                    if (!AddressFloorInfo.Equals(string.Empty))
                    {
                        NewAddress.Number = NewAddress.Number + ", " + AddressFloorInfo;
                    }

                }
                else
                {
                    ThereIsAddressNumberOrDoesNotWantTo = false;
                    await NavigationService.ShowPopupAsync(RequestAskForAddressNumberPopupVM);
                }

            }

            if (IsAddressGettingSaved && ThereIsAddressNumberOrDoesNotWantTo == true)
            {
                // Save new home address (with address number)
                NewAddress.Number = AddressNumberInfo;
                await NavigationService.NavigateToAsync<NewHomeRequestChooseTypeViewModel>(NewAddress);

            }
            else if (!IsAddressGettingSaved && ThereIsAddressNumberOrDoesNotWantTo == true)
            {
                // pass home address without saving it (with address number)
                NewAddress.Number = AddressNumberInfo;
                await NavigationService.NavigateToAsync<NewHomeRequestChooseTypeViewModel>(NewAddress);
            }


        }


        private Command _resetEntryTextCommand;
        public ICommand ResetEntryTextCommand
        {
            get
            {
                return _resetEntryTextCommand ??
                    (_resetEntryTextCommand = new Command(OnResetEntryTextCommand, (o) => !IsBusy));
            }
        }

        private void OnResetEntryTextCommand(object obj)
        {
            SugestedAddressesList.Clear();
            Address = string.Empty;
            AddressSelected = null;
        }


        private Command _addressEntryListenerCommand;
        public ICommand AddressEntryListenerCommand
        {
            get
            {
                return _addressEntryListenerCommand ??
                    (_addressEntryListenerCommand = new Command(OnAddressEntryListenerCommand, (o) => !IsBusy));
            }
        }

        private void OnAddressEntryListenerCommand(object obj)
        {
            string result = string.Empty;
            if (obj is string)
            {
                result = (string)obj;
            }

            if (result == string.Empty)
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
                else
                {
                    SugestedAddressesList.Clear();
                }
                IsSearchIconVisible = true;
            }


        }


        private Command _switchHomeAddressCommand;
        public ICommand SwitchHomeAddressCommand
        {
            get
            {
                return _switchHomeAddressCommand ??
                    (_switchHomeAddressCommand = new Command(OnSwitchHomeAddress, (o) => !IsBusy));
            }
        }

        private void OnSwitchHomeAddress(object obj)
        {
            bool result = false;
            if (obj is bool)
                result = (bool)obj;

            if (result == true)
            {
                IsAddressGettingSaved = true;
            }
            else
            {
                IsAddressGettingSaved = false;
            }
        }
        #endregion

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

        #region Address related properties

        private ValidatableObject<string> _addressName = new ValidatableObject<string>();

        public ValidatableObject<string> AddressName
        {
            get { return _addressName; }
            set { Set(ref _addressName, value); }
        }



        private AddressModel _addressSelected;

        public AddressModel AddressSelected
        {
            get { return _addressSelected; }
            set { Set(ref _addressSelected, value); }
        }
        private AddressModel _newAddress = new AddressModel();

        public AddressModel NewAddress
        {
            get { return _newAddress; }
            set { Set(ref _newAddress, value); }
        }

        private string _addressNumberInfo = string.Empty;

        public string AddressNumberInfo
        {
            get { return _addressNumberInfo; }
            set { Set(ref _addressNumberInfo, value); }
        }

        private string _addressPortalInfo = string.Empty;

        public string AddressPortalInfo
        {
            get { return _addressPortalInfo; }
            set { Set(ref _addressPortalInfo, value); }
        }
        private string _addressFloorInfo = string.Empty;

        public string AddressFloorInfo
        {
            get { return _addressFloorInfo; }
            set { Set(ref _addressFloorInfo, value); }
        }



        private string _address = string.Empty;

        public string Address
        {
            get { return _address; }
            set { Set(ref _address, value); }
        }

        private bool _isAddressGettingSaved;

        public bool IsAddressGettingSaved
        {
            get { return _isAddressGettingSaved; }
            set { Set(ref _isAddressGettingSaved, value); }
        }


        #endregion

        public RequestAskForAddressNumberPopupViewModel RequestAskForAddressNumberPopupVM { get; private set; }


        private bool _thereIsAddressNumberOrDoesNotWantTo;

        public bool ThereIsAddressNumberOrDoesNotWantTo
        {
            get { return _thereIsAddressNumberOrDoesNotWantTo; }
            set { Set(ref _thereIsAddressNumberOrDoesNotWantTo, value); }
        }


        private bool _saveHomeAddressSwitch;

        public bool SaveHomeAddressSwitch
        {
            get { return _saveHomeAddressSwitch; }
            set { Set(ref _saveHomeAddressSwitch, value); }
        }

        private ObservableCollection<AddressModel> _sugestedAddressesList = new ObservableCollection<AddressModel>();
        public ObservableCollection<AddressModel> SugestedAddressesList
        {
            get { return _sugestedAddressesList; }
            set { Set(ref _sugestedAddressesList, value); }
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

            return returnValue;
        }

        public Task RefreshSugestedAddressesAsync(string result)
        {
            return ExecuteOperationAsync(async () =>
            {
                SugestedAddressesList = new ObservableCollection<AddressModel>();

                var places = await _googleMapsPlaceService.FindPlacesAsync(result);
                SugestedAddressesList.AddRange(places);
            });
        }

        #endregion

    }
}
