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

        }

        protected override void OnAppearing()
        {
            MessagingCenter.Subscribe<DashboardTabMessage>(this, string.Empty, OnTabChanged);
            ViewModel?.ActivateAsync();
        }

        void OnTabChanged(DashboardTabMessage message)
        {
            SelectedItem = CurrentPage;
            SelectedItem = Children[(int)message.Message];
        }

        protected override void OnDisappearing()
        {
            MessagingCenter.Unsubscribe<DashboardTabMessage>(this, string.Empty);
            ViewModel?.Deactivate();
        }
    }
}