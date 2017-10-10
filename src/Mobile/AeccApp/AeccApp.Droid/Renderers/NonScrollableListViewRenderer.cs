using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using AeccApp.Core.Controls;
using AeccApp.Droid.Renderers;
using Xamarin.Forms.Platform.Android;
using System.Reflection;

[assembly: ExportRenderer(typeof(NonScrollableListView), typeof(NonScrollableListViewRenderer))]
namespace AeccApp.Droid.Renderers
{
    public class NonScrollableListViewRenderer : ListViewRenderer
    {
        public static void Initialize()
        {
            var test = DateTime.UtcNow;

        }
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
