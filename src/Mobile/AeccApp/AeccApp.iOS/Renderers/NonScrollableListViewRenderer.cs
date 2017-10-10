using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;
using AeccApp.Core.Controls;
using AeccApp.iOS.Renderers;

[assembly: ExportRenderer(typeof(NonScrollableListView), typeof(NonScrollableListViewRenderer))]
namespace AeccApp.iOS.Renderers
{
   public class NonScrollableListViewRenderer : ListViewRenderer
    {
        public static void Initialize()
        {
        }
        /// <summary>
        /// Deactivates Scroll of NonScrollableListView control
        /// </summary>
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.ListView> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
                Control.ScrollEnabled = false;

        }
    }
}
