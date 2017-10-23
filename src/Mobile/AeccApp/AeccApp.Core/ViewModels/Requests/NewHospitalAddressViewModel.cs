using AeccApp.Core.Models;
using AeccApp.Core.ViewModels.Popups;
using AeccApp.Core.Views.Popups;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace AeccApp.Core.ViewModels
{
    public class NewHospitalAddressViewModel : ViewModelBase
    {


        #region Commands


        private Command _hospitalMapTabCommand;
        public ICommand HospitalMapTabCommand
        {
            get
            {
                return _hospitalMapTabCommand ??
                    (_hospitalMapTabCommand = new Command(OnHospitalMapTabCommand, (o) => !IsBusy));
            }
        }

        void OnHospitalMapTabCommand(object obj)
        {
            SwitchBetweenAndHospitalList = true;
         
        }
        private Command _hospitalListTabCommand;
        public ICommand HospitalListTabCommand
        {
            get
            {
                return _hospitalListTabCommand ??
                    (_hospitalListTabCommand = new Command(OnHospitalListTabCommand, (o) => !IsBusy));
            }
        }

        void OnHospitalListTabCommand(object obj)
        {
            SwitchBetweenAndHospitalList = false;

        }

        

        private Command _newHospitalSelectedCommand;
        public ICommand NewHospitalSelectedCommand
        {
            get
            {
                return _newHospitalSelectedCommand ??
                    (_newHospitalSelectedCommand = new Command(OnNewHospitalSelectedCommand, (o) => !IsBusy));
            }
        }

        async void OnNewHospitalSelectedCommand(object obj)
        {
            await NavigationService.NavigateToAsync<NewRequestSelectAddressViewModel>();
        }


        private Command _relocateCommand;
        public ICommand RelocateCommand
        {
            get
            {
                return _relocateCommand ??
                    (_relocateCommand = new Command(OnRelocateCommand, (o) => !IsBusy));
            }
        }

        async void OnRelocateCommand(object obj)
        {
            //TODO UPDATE GEOLOCALIZATION
            RequestHospitalAskForRoomPopupViewModel vmTest = new RequestHospitalAskForRoomPopupViewModel();


            await NavigationService.NavigateToAsync<CompletingHospitalRequestViewModel>(new AddressModel("name","street","province","2","floor2","placeidd",new Models.Requests.Position(2.0,2.3)));

        }



        #endregion

        #region Properties


        private bool _switchBetweenAndHospitalList;

        public bool SwitchBetweenAndHospitalList
        {
            get { return _switchBetweenAndHospitalList; }
            set { Set(ref _switchBetweenAndHospitalList, value); }
        }

  
        #endregion





    }
}
