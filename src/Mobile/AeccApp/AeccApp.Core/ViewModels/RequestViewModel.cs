using AeccApp.Internationalization.Properties;
using System.Resources;
using System.Windows.Input;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace AeccApp.Core.ViewModels
{
    public class RequestsViewModel : ViewModelBase
    {

        #region Init
        public override Task ActivateAsync()
        {
            FirstTimeLandingPageVisible = Settings.FirstRequestLandingPageSeen;
          
            return Task.CompletedTask;
        }
     

        #endregion

        #region Commands
        private Command _newRequestCommand;
        public ICommand NewRequestCommand
        {
            get
            {
                return _newRequestCommand ??
                    (_newRequestCommand = new Command(OnNewRequestCommand, o => !IsBusy));
            }
        }
        /// <summary>
        /// Navigates to NewRequestSelectAddressView
        /// </summary>
        /// <param name="obj"></param>
         void OnNewRequestCommand(object obj)
        {
            Settings.FirstRequestLandingPageSeen = true;
            FirstTimeLandingPageVisible = true;
        }

        private Command _atHomeCommand;
        public ICommand AtHomeCommand
        {
            get
            {
                return _atHomeCommand ??
                    (_atHomeCommand = new Command(OnAtHomeCommand, o => !IsBusy));
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
                    (_atHospitalCommand = new Command(OnAtHospitalCommand, o => !IsBusy));
            }
        }

        async void OnAtHospitalCommand(object obj)
        {
            await NavigationService.NavigateToAsync<HospitalAddressesListViewModel>();


        }



        #endregion

        private bool _firstTimeLandingPageVisible;

        public bool FirstTimeLandingPageVisible
        {
            get { return _firstTimeLandingPageVisible; }
            set { Set(ref _firstTimeLandingPageVisible, value); }
        }



    }
}
