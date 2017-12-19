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
        public IAddressesDataService HomeAddressesDataService { get; } = ServiceLocator.HomeAddressesDataService;

        #region Activate & Deactive Methods
        public HomeAddressesListViewModel()
        {
            HomeAddressesList = new ObservableCollection<AddressModel>();
        }

        public override Task ActivateAsync()
        {
            return ExecuteOperationAsync(async () =>
            {
                var homeAddresses = await HomeAddressesDataService.GetListAsync();
                HomeAddressesList.SyncExact(homeAddresses.Where(x=> !x.IsHospitalAddress).ToList());
                HomeAddressesIsEmpty = !HomeAddressesList.Any();
            });
        }
        #endregion

        #region Properties
        public ObservableCollection<AddressModel> HomeAddressesList { get; private set; }
        
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
                    (_chooseAddressCommand = new Command(OnChooseAddress, o => !IsBusy));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj">obj contains the HomeAddressList item tapped</param>
        private async void OnChooseAddress(object obj)
        {
            var selectedAddress = obj as AddressModel;
            await NavigationService.NavigateToAsync<HomeRequestChooseTypeViewModel>(selectedAddress);
        }

        private Command _newHomeAddressCommand;
        public ICommand NewHomeAddressCommand
        {
            get
            {
                return _newHomeAddressCommand ??
                    (_newHomeAddressCommand = new Command(OnNewHomeAddressCommand, o => !IsBusy));
            }
        }

        async void OnNewHomeAddressCommand(object obj)
        {
            await NavigationService.NavigateToAsync<NewHomeAddressViewModel>();
        }


        #endregion
    }
}
