using AeccApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

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
          //  NewsFlowListView.HeightRequest = GetGridContainerHeight(6, 2, NewsFlowListView.RowHeight);
        }



        public static double GetGridContainerHeight(double itemCount, double columnCount, int rowHeight)
        {
            var rowCount = Math.Ceiling(itemCount / columnCount);
            return rowCount * rowHeight;
        }


    }
}