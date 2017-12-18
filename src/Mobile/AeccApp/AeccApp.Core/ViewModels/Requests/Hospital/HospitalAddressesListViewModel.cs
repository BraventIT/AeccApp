using AeccApp.Core.Extensions;
using AeccApp.Core.Models;
using AeccApp.Core.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AeccApp.Core.ViewModels
{
    public class HospitalAddressesListViewModel : ViewModelBase
    {
        private IAddressesDataService AddressesDataService { get; } = ServiceLocator.HomeAddressesDataService;

        #region Activate & Deactive Methods
        public HospitalAddressesListViewModel()
        {
            HospitalAddressesList = new ObservableCollection<AddressModel>();
        }

        public override Task ActivateAsync()
        {
            return ExecuteOperationAsync(async () =>
            {
                var hospitalAddresses = await AddressesDataService.GetListAsync();
                HospitalAddressesList.SyncExact(hospitalAddresses);
                HospitalAddressesIsEmpty = !hospitalAddresses.Any();
            });
        }
        #endregion

        #region Properties
        public ObservableCollection<AddressModel> HospitalAddressesList { get; private set; }

        private bool _hospitalAddressesIsEmpty;
        public bool HospitalAddressesIsEmpty
        {
            get { return _hospitalAddressesIsEmpty; }
            set { Set(ref _hospitalAddressesIsEmpty, value); }
        }
        #endregion

        #region Commands

        private Command _chooseHospitalCommand;
        public ICommand ChooseHospitalCommand
        {
            get
            {
                return _chooseHospitalCommand ??
                    (_chooseHospitalCommand = new Command(OnChooseHospitalCommand, o => !IsBusy));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj">obj contains the HomeAddressList item tapped</param>
        private async void OnChooseHospitalCommand(object obj)
        {
            var selectedAddress = obj as AddressModel;
            await NavigationService.NavigateToAsync<HospitalRequestChooseTypeViewModel>(selectedAddress);
        }

        private Command _newHospitalAddressCommand;
        public ICommand NewHospitalAddressCommand
        {
            get
            {
                return _newHospitalAddressCommand ??
                    (_newHospitalAddressCommand = new Command(OnNewHospitalAddressCommand, o => !IsBusy));
            }
        }

        async void OnNewHospitalAddressCommand(object obj)
        {
            await NavigationService.NavigateToAsync<NewHospitalAddressViewModel>();
        }


        #endregion
    }
}
