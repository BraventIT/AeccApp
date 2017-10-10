using AeccApp.Core.Controls;
using AeccApp.UWP.CustomRenderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Xamarin.Forms.Platform.UWP;
using System.ComponentModel;

[assembly: ExportRenderer(typeof(NonScrollableListView), typeof(NonScrollableListViewRenderer))]
namespace AeccApp.UWP.CustomRenderer
{
    public class NonScrollableListViewRenderer : ListViewRenderer
    {
        public static void Initialize()
        {

        }
        /// <summary>
        /// This renderer deactivates scroll of the NonScrollableListView control
        /// applying a style created to do so on UWP's App.xaml
        /// </summary>
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.ListView> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                var listView = Control as ListView;
                listView.Style = (Style)Application.Current.Resources["RemoveScrollVisibility"];   
            }
        }


    }
}
