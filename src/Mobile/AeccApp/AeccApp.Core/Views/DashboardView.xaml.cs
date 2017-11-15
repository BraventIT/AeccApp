using AeccApp.Core.Messages;
using AeccApp.Core.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AeccApp.Core.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DashboardView : TabbedPage
    {
        protected INavigableViewModel ViewModel => BindingContext as INavigableViewModel;
        private bool HasBeenAlreadyInitialized = false;

        public DashboardView()
        {
            InitializeComponent();

        }

        protected override void OnAppearing()
        {
            MessagingCenter.Subscribe<DashboardEnableAndDisableChatTab>(this, string.Empty, OnEnableAndDisableChatTab);
            MessagingCenter.Subscribe<DashboardHideRequestsTabMessage>(this, string.Empty, OnHideRequestsTab);
            MessagingCenter.Subscribe<DashboardTabMessage>(this, string.Empty, OnTabChanged);
            MessagingCenter.Send(new ToolbarMessage(true), string.Empty);
            ViewModel?.ActivateAsync();
        }

        void OnEnableAndDisableChatTab(DashboardEnableAndDisableChatTab message)
        {
            Children[(int)TabsEnum.Chat].IsEnabled =  message.Message;
        }
        void OnHideRequestsTab(DashboardHideRequestsTabMessage message)
        {
            if(HasBeenAlreadyInitialized == false)
            {
               Children.Remove(Children[(int)message.Message]);
                HasBeenAlreadyInitialized = true;
            }

        }

        void OnTabChanged(DashboardTabMessage message)
        {
            SelectedItem = CurrentPage;
            SelectedItem = Children[(int)message.Message];
        }

        protected override void OnDisappearing()
        {
            MessagingCenter.Unsubscribe<DashboardEnableAndDisableChatTab>(this, string.Empty);
            MessagingCenter.Unsubscribe<DashboardHideRequestsTabMessage>(this, string.Empty);
            MessagingCenter.Unsubscribe<DashboardTabMessage>(this, string.Empty);
            MessagingCenter.Send(new ToolbarMessage(false), string.Empty);
            ViewModel?.Deactivate();
        }
    }
}