using AeccApp.Internationalization.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AeccApp.Core.ViewModels
{
    public class NewRequestSelectAddressViewModel : ViewModelBase
    {

     
        #region Commands
        private Command _atHomeCommand;
        public ICommand AtHomeCommand
        {
            get
            {
                return _atHomeCommand ??
                    (_atHomeCommand = new Command(OnAtHomeCommand, (o) => !IsBusy));
            }
        }

        async void OnAtHomeCommand(object obj)
        {
            await NavigationService.NavigateToAsync<HomeAddressesListViewModel>();


        }


        private Command _atHospitalCommand;
        public ICommand AtHospitalCommand
        {
            get
            {
                return _atHospitalCommand ??
                    (_atHospitalCommand = new Command(OnAtHospitalCommand, (o) => !IsBusy));
            }
        }

        async void OnAtHospitalCommand(object obj)
        {
            await NavigationService.NavigateToAsync<NewRequestSelectAddressViewModel>();


        }


        #endregion

       
    }
}
