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

        public DashboardView()
        {
            InitializeComponent();
            if (GlobalSetting.Instance.User?.IsVolunteer ?? false)
            {
                Children.Remove(Children[(int)TabsEnum.Requests]);
            }
        }

        protected override void OnAppearing()
        {
            MessagingCenter.Subscribe<DashboardTabMessage>(this, string.Empty, OnTabChanged);
            MessagingCenter.Subscribe<ToolbarMessage>(this, string.Empty, OnToolbarChanged);
           
            ViewModel?.ActivateAsync();
        }

        private void OnToolbarChanged(ToolbarMessage message)
        {
            Title = (message.ShowLogo) ?
                string.Empty : message.Title;
        }


        void OnTabChanged(DashboardTabMessage message)
        {
            SelectedItem = CurrentPage;
            SelectedItem = Children[(int)message.Message];
        }

        protected override void OnDisappearing()
        {
            MessagingCenter.Unsubscribe<DashboardTabMessage>(this, string.Empty);
            MessagingCenter.Unsubscribe<ToolbarMessage>(this, string.Empty);
            ViewModel?.Deactivate();
        }
    }
}