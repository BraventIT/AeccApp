using AeccApp.Core.Extensions;
using AeccApp.Core.Models;
using AeccApp.Core.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AeccApp.Core.ViewModels
{
    public class HospitalAddressesListViewModel : ViewModelBase
    {
        public readonly IHomeAddressesDataService _homeAddressesDataService;

        #region Activate & Deactive Methods
        public HospitalAddressesListViewModel()
        {
            _homeAddressesDataService = ServiceLocator.HomeAddressesDataService;

            _hospitalAddressesList = new ObservableCollection<AddressModel>();
        }

        public override Task ActivateAsync()
        {
            return ExecuteOperationAsync(async () =>
            {
                var hospitalAddresses = await _homeAddressesDataService.GetListAsync();

                if (hospitalAddresses != null)
                {
                    HospitalAddressesList.Clear();
                    HospitalAddressesList.AddRange(hospitalAddresses.Where(o => o.IsHospitalAddress));               
                }
                HospitalAddressesIsEmpty = hospitalAddresses == null || !hospitalAddresses.Any();
            });
        }
        #endregion

        #region Properties
        private ObservableCollection<AddressModel> _hospitalAddressesList;

        public ObservableCollection<AddressModel> HospitalAddressesList
        {
            get { return _hospitalAddressesList; }
            set { Set(ref _hospitalAddressesList, value); }
        }

        private bool _hospitalAddressesIsEmpty;
        public bool HospitalAddressesIsEmpty
        {
            get { return _hospitalAddressesIsEmpty; }
            set { Set(ref _hospitalAddressesIsEmpty, value); }
        }
        #endregion

        #region Commands


        private Command _newHospitalAddressCommand;
        public ICommand NewHospitalAddressCommand
        {
            get
            {
                return _newHospitalAddressCommand ??
                    (_newHospitalAddressCommand = new Command(OnNewHospitalAddressCommand, (o) => !IsBusy));
            }
        }

        async void OnNewHospitalAddressCommand(object obj)
        {
            await NavigationService.NavigateToAsync<NewHospitalAddressViewModel>();
        }


        #endregion



    }
}
