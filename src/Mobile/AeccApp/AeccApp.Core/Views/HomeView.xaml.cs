using AeccApp.Core.Messages;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AeccApp.Core.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomeView : BaseContentPage
    {
        public HomeView()
        {
            InitializeComponent();
           // NewsFlowListView.HeightRequest = GetGridContainerHeight(4, 2, NewsFlowListView.RowHeight);
        }



        public static double GetGridContainerHeight(double itemCount, double columnCount, int rowHeight)
        {
            var rowCount = Math.Ceiling(itemCount / columnCount);
            return rowCount * rowHeight;
        }

        protected override void OnAppearing()
        {
            var toolbarMessage = (Device.RuntimePlatform != Device.UWP) ?
               new ToolbarMessage(true) :
               new ToolbarMessage(this["DashboardAppTitle"]);

            MessagingCenter.Send(toolbarMessage, string.Empty);

            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            MessagingCenter.Send(new ToolbarMessage(false), string.Empty);
            base.OnDisappearing();
        }

    }
}