using AeccApp.Core.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;
using System.Threading.Tasks;
using AeccApp.Core.Services;
using AeccApp.Core.Extensions;

namespace AeccApp.Core.ViewModels
{
    public class HomeAddressesListViewModel : ViewModelBase
    {
        public readonly IHomeAddressesDataService _homeAddressesDataService;

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
            });
        }

        #region Commands


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

        #region Properties
        private ObservableCollection<AddressModel> _homeAddressesList;

        public ObservableCollection<AddressModel> HomeAddressesList
        {
            get { return _homeAddressesList; }
            set { Set(ref _homeAddressesList, value); }
        }

        #endregion

    }
}
