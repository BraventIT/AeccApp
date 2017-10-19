using AeccApp.Core.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;
using System.Threading.Tasks;
using AeccApp.Core.Services;
using AeccApp.Core.Extensions;
using System.Linq;

namespace AeccApp.Core.ViewModels
{
    public class HomeAddressesListViewModel : ViewModelBase
    {
        public readonly IHomeAddressesDataService _homeAddressesDataService;

        #region Activate & Deactive Methods
        public HomeAddressesListViewModel()
        {
            _homeAddressesDataService = ServiceLocator.HomeAddressesDataService;

            _homeAddressesList = new ObservableCollection<AddressModel>();
        }

        public override Task ActivateAsync()
        {
            return ExecuteOperationAsync(async () =>
            {
                var homeAddresses = await _homeAddressesDataService.GetListAsync();

                if (homeAddresses != null)
                {
                    HomeAddressesList.Clear();
                    HomeAddressesList.AddRange(homeAddresses);
                }
                HomeAddressesIsEmpty = homeAddresses == null || !homeAddresses.Any();
            });
        }
        #endregion

        #region Properties
        private ObservableCollection<AddressModel> _homeAddressesList;

        public ObservableCollection<AddressModel> HomeAddressesList
        {
            get { return _homeAddressesList; }
            set { Set(ref _homeAddressesList, value); }
        }

        private bool _homeAddressesIsEmpty;
        public bool HomeAddressesIsEmpty
        {
            get { return _homeAddressesIsEmpty; }
            set { Set(ref _homeAddressesIsEmpty, value); }
        }
        #endregion

        #region Commands

        private Command _chooseAddressCommand;
        public ICommand ChooseAddressCommand
        {
            get
            {
                return _chooseAddressCommand ??
                    (_chooseAddressCommand = new Command(OnChooseAddress, (o) => !IsBusy));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj">obj contains the HomeAddressList item tapped</param>
        private async void OnChooseAddress(object obj)
        {
            var selectedAddress = obj as AddressModel;
            await NavigationService.NavigateToAsync<NewHomeRequestChooseTypeViewModel>(selectedAddress);
        }

        private Command _newHomeAddressCommand;
        public ICommand NewHomeAddressCommand
        {
            get
            {
                return _newHomeAddressCommand ??
                    (_newHomeAddressCommand = new Command(OnNewHomeAddressCommand, (o) => !IsBusy));
            }
        }

        async void OnNewHomeAddressCommand(object obj)
        {
            await NavigationService.NavigateToAsync<NewHomeAddressViewModel>();

        }


        #endregion

      

    }
}
