using AeccApp.Core.Models.Requests;
using AeccApp.Internationalization.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AeccApp.Core.ViewModels
{
    public class HomeAddressesListViewModel : ViewModelBase
    {
        
       

        public HomeAddressesListViewModel()
        {
            //TODO Delete mock home addresses
            HomeAddressesList = new ObservableCollection<AddressModel>();
            HomeAddressesList.Add(new AddressModel("Mi casa", "Fake street", "Castellón", "1234", "2", "Fake street, 1234 2ºa, Madrid", "placeID"));
            HomeAddressesList.Add(new AddressModel("Mi casa", "Fake street", "Castellón", "1234", "2", "Fake street, 1234 2ºa, Madrid", "placeID"));
            HomeAddressesList.Add(new AddressModel("Mi casa", "Fake street", "Castellón", "1234", "2", "Fake street, 1234 2ºa, Madrid", "placeID"));
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
