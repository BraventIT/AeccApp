using AeccApp.Core.Messages;
using AeccApp.Core.Models;
using AeccApp.Core.Services;
using AeccApp.Core.ViewModels.Popups;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AeccApp.Core.ViewModels
{
    class HomeViewModel : ViewModelBase
    {
        private readonly IChatService ChatService;

        #region Contructor & Initialize
        public HomeViewModel()
        {
            IsHeaderInfoBannerVisible = Settings.HomeHeaderBannerClosed;
            ChatService = ServiceLocator.ChatService;
        }

        public override async Task ActivateAsync()
        {
            MessagingCenter.Subscribe<ChatMessageReceivedMessage>(this, string.Empty, OnMessageReceived);
            MessagingCenter.Subscribe<ChatStateMessage>(this, string.Empty, OnChatState);

            await ExecuteOperationAsync(async () =>
            {
                await ChatService.InitializeAsync();

                VolunteerIsActive = ChatService.VolunteerIsActive;
                InConversation = ChatService.InConversation;
            });
        }

        public override void Deactivate()
        {
            MessagingCenter.Unsubscribe<ChatMessageReceivedMessage>(this, string.Empty);
            MessagingCenter.Unsubscribe<ChatStateMessage>(this, string.Empty);
        }
        #endregion

        #region Propeties
        private bool _isHeaderInfoBannerVisible;
        public bool IsHeaderInfoBannerVisible
        {
            get { return _isHeaderInfoBannerVisible; }
            set
            {
                Set(ref _isHeaderInfoBannerVisible, value);
            }
        }

        private bool inConversation;
        public bool InConversation
        {
            get { return inConversation; }
            set
            {
                Set(ref inConversation, value);
            }
        }

        private Message lastMessage;
        public Message LastMessage
        {
            get { return lastMessage; }
            set { Set(ref lastMessage, value); }
        }

        public List<NewsModel> NewsList
        {
            get
            {
                return new List<NewsModel>()
                {
                    new NewsModel("id1", null, "La FEHM y la AECC firman un convenio para la promoción de actividades orientadas a la prevención, información y concienciación sobre el cáncer", "La presidenta ejecutiva de la Federación Empresarial Hotelera de Mallorca (FEHM), Inmaculada de Benito, y el presidente de la Asociación Española contra el Cáncer de Baleares (AECC), Javier Cortés, han firmado un convenio de colaboración con el objetivo de promover una serie de acciones dirigidas tanto a los trabajadores  de las empresas asociadas a la Federación como a los clientes de los establecimientos hoteleros, que incidan en la prevención, información y concienciación del cáncer, así como la promoción de actividades orientadas a la lucha contra esta enfermedad"),
                    new NewsModel("id2",null, "El cantante Dani Fernández, acompañado por Tony Aguilar, da nombre a una nueva sala infantil del 12 de Octubre.", "El presentador y locutor Tony Aguilar y el joven cantante y compositor y ex miembro del grupo Auryn Dani Fernández visitaron la planta de Oncología Infantil del Hospital 12 de Octubre, de Madrid, para mostrar su solidaridad con los pequeños pacientes hospitalizados y protagonizar la remodelación de la sala de espera infantil de Medicina Nuclear."),
                    new NewsModel("id3", null, "La AECC presenta el World Cancer Research Day en ESMO", "El Día Mundial de la Investigación en Cáncer (WCRD en sus siglas en inglés), la iniciativa impulsada por la Asociación Española Contra el Cáncer (AECC) que se puso en marcha el año pasado y que tiene como objetivo promover un movimiento global a favor de la investigación oncológica, inaugura su campaña 2017 con un acto enmarcado dentro de la celebración del congreso de la Sociedad Europea de Oncología Médica, ESMO 2017. Este acto, que lleva por título Oncología e investigación en España y su entorno: compartiendo conocimiento en cáncer."),
                    new NewsModel("id4", null, "La AECC presenta el World Cancer Research Day en ESMO", "El Día Mundial de la Investigación en Cáncer (WCRD en sus siglas en inglés), la iniciativa impulsada por la Asociación Española Contra el Cáncer (AECC) que se puso en marcha el año pasado y que tiene como objetivo promover un movimiento global a favor de la investigación oncológica, inaugura su campaña 2017 con un acto enmarcado dentro de la celebración del congreso de la Sociedad Europea de Oncología Médica, ESMO 2017. Este acto, que lleva por título Oncología e investigación en España y su entorno: compartiendo conocimiento en cáncer."),
              
                };
            }
        }

        private bool _volunteerIsActive;
        public bool VolunteerIsActive
        {
            get { return _volunteerIsActive; }
            set
            {
                if (Set(ref _volunteerIsActive, value) && !IsBusy)
                {
                    OnVolunteerStateChangedAsync(value);
                }
            }
        }

        #endregion

        #region Popups Properties


        private bool eventPopupVisible;
        public bool EventPopupVisible
        {
            get { return eventPopupVisible; }
            set { Set(ref eventPopupVisible, value); }
        }


        private bool checkBoxImageEnabled;

        public bool CheckBoxImageEnabled
        {
            get { return checkBoxImageEnabled; }
            set { checkBoxImageEnabled = value; }
        }


        #endregion

        #region Commands
        private Command _headerInfoBannerCall;
        public ICommand HeaderInfoBannerCall
        {
            get
            {
                return _headerInfoBannerCall ??
                    (_headerInfoBannerCall = new Command(OnHeaderInfoBannerCall));
            }
        }

        void OnHeaderInfoBannerCall()
        {
            Device.OpenUri(new Uri("tel://900100036"));
        }

        private Command _headerInfoBannerClose;
        public ICommand HeaderInfoBannerClose
        {
            get
            {
                return _headerInfoBannerClose ??
                    (_headerInfoBannerClose = new Command(OnHeaderInfoBannerClose));
            }
        }

        void OnHeaderInfoBannerClose()
        {
            IsHeaderInfoBannerVisible = false;
            Settings.HomeHeaderBannerClosed = false;

        }

        private Command _openAllRequestsCommand;
        public ICommand OpenAllRequestsCommand
        {
            get
            {
                return _openAllRequestsCommand ??
                    (_openAllRequestsCommand = new Command(OnOpenAllRequest));
            }
        }

        async void OnOpenAllRequest()
        {
            //TODO Navigate to AllYourRequestListView if there are active requests
            //TODO Navigate to RequestsView if there are not active requests

            await NavigationService.NavigateToAsync<AllYourRequestsListViewModel>();
        }

        private Command _newRequestCommand;
        public ICommand NewRequestCommand
        {
            get
            {
                return _newRequestCommand ??
                    (_newRequestCommand = new Command(OnNewRequest));
            }
        }
        /// <summary>
        /// Navigates to NewRequestSelectAddressView
        /// </summary>
        void OnNewRequest()
        {
            NavigationService.NavigateToAsync<NewRequestSelectAddressViewModel>();
        }
        private Command _currentChatCommand;
        public ICommand CurrentChatCommand
        {
            get
            {
                return _currentChatCommand ??
                    (_currentChatCommand = new Command(OnCurrentChatCommand));
            }
        }

        void OnCurrentChatCommand()
        {
            MessagingCenter.Send(new DashboardTabMessage(TabsEnum.Chat), string.Empty);
        }

        private Command _newChatCommand;
        public ICommand NewChatCommand
        {
            get
            {
                return _newChatCommand ??
                    (_newChatCommand = new Command(OnNewChat));
            }
        }
     

        async void OnNewChat()
        {
          
                MessagingCenter.Send(new DashboardTabMessage(TabsEnum.Chat), string.Empty);
            
        }


        private Command openAllNewsCommand;
        public ICommand OpenAllNewsCommand
        {
            get
            {
                return openAllNewsCommand ??
                    (openAllNewsCommand = new Command(o=> OnOpenAllNewsViewAsync()));
            }
        }
        /// <summary>
        /// Navigates to AllNewsView
        /// </summary>
        private async Task OnOpenAllNewsViewAsync()
        {
            await NavigationService.NavigateToAsync<AllNewsViewModel>();
        }

        private Command chooseNewCommand;
        public ICommand ChooseNewCommand
        {
            get
            {
                return chooseNewCommand ??
                    (chooseNewCommand = new Command(o => OnChooseNewAsync(o), o => !IsBusy));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj">obj contains the NewsList item tapped</param>
        private async Task OnChooseNewAsync(object obj)
        {
            var selectedNew = obj as NewsModel;
            await NavigationService.NavigateToAsync<NewsDetailViewModel>(selectedNew);
        }

        #endregion

        #region Private Methods

        public override bool OnBackButtonPressed()
        {
            bool returnValue = false;
            if (EventPopupVisible)
            {
                EventPopupVisible = false;
                returnValue = true;
            }
         

            return returnValue;
        }


        private void OnMessageReceived(ChatMessageReceivedMessage received)
        {
            LastMessage = received.Message;
        }

        private void OnChatState(ChatStateMessage obj)
        {
            InConversation = obj.InConversation;
            VolunteerIsActive = obj.VolunteerIsActive;
        }

        Task OnVolunteerStateChangedAsync(bool isActive)
        {
            return ExecuteOperationAsync(() => ChatService.SetVolunteerState(isActive),
                finallyAction: () => VolunteerIsActive = ChatService.VolunteerIsActive);
        }
        #endregion
    }
}
