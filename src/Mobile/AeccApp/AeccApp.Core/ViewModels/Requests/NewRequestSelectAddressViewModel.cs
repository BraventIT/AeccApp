using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AeccApp.Core.ViewModels
{
    public class NewRequestSelectAddressViewModel : ViewModelBase
    {

        public async override Task ActivateAsync()
        {
            //TESTS TO DELETE
            //string[] destinatarios = { "acabrera@bravent.net" };
            //await ExecuteOperationAsync(async (token) =>
            //{
            //    await ServiceLocator.EmailService.SendAsync(
            //        new EmailFromHome(new Models.RequestModel(new RequestType(),
            //        "requestlocation", "15/08/2017", "time", "comentarios blablablo",
            //        new Models.AddressModel
            //        ("Casa parla", "Calle de blabla", "Madrid", "12", "1", "afzf", new Xamarin.Forms.GoogleMaps.Position(0, 0)))
            //        , destinatarios), token);
            //});
            //TESTS TO DELETE

        }

        #region Commands
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


    }
}
