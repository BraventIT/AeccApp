using Xamarin.Forms;
using AeccApp.Droid.Renderers;
using Xamarin.Forms.Platform.Android;
using DLToolkit.Forms.Controls;

[assembly: ExportRenderer(typeof(FlowListView), typeof(NonScrollableListViewRenderer))]
namespace AeccApp.Droid.Renderers
{
    public class NonScrollableListViewRenderer : ListViewRenderer
    {
       
        /// <summary>
        /// Deactivates Scroll of NonScrollableListView control
        /// </summary>
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.ListView> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {

                Control.VerticalScrollBarEnabled = false;
             

            }
            


        }
    }
}
