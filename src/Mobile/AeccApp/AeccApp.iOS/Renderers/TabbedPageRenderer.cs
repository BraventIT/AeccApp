using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using Xamarin.Forms;
using AeccApp.Core.Views;
using AeccApp.iOS.Renderers;
using Xamarin.Forms.Platform.iOS;
using CoreGraphics;
using AeccApp.Core.Resources;

[assembly: ExportRenderer(typeof(TabbedPage), typeof(TabbedPageRenderer))]
namespace AeccApp.iOS.Renderers
{
    public class TabbedPageRenderer : TabbedRenderer
    {
        public TabbedPageRenderer()
        {
            TabBar.TintColor = Color.FromHex(ResourcesReference.APPBAR_ACCENT_COLOR).ToUIColor();
            TabBar.UnselectedItemTintColor = Color.FromHex(ResourcesReference.NAV_INACTIVE_COLOR).ToUIColor();
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();

            if (NavigationController != null)
            {
                
                NavigationController.NavigationBar.Frame = new CGRect(0, 0, View.Frame.Size.Width, 72.0);
                NavigationController.NavigationBar.SetTitleVerticalPositionAdjustment(-5, UIBarMetrics.Default);
                var navigationItem = NavigationController.TopViewController.NavigationItem;
                var leftNativeButtons = (navigationItem.LeftBarButtonItems ?? new UIBarButtonItem[] { }).ToList();
                var rightNativeButtons = (navigationItem.RightBarButtonItems ?? new UIBarButtonItem[] { }).ToList();

                leftNativeButtons.ForEach(x => x.ImageInsets = new UIEdgeInsets(-8, 0, 0, 0));
                rightNativeButtons.ForEach(x => x.ImageInsets = new UIEdgeInsets(-8, 0, 0, 0));
            }
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            var image = UIImage.FromBundle("logoHeader.png");
            var imageView = new UIImageView(new CGRect(0, 0, 140, 70));

            imageView.ContentMode = UIViewContentMode.ScaleAspectFit;
            imageView.Image = image.ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal);

            if (NavigationController != null)
            {
                NavigationController.TopViewController.NavigationItem.TitleView = imageView;
                NavigationController.NavigationBar.BarTintColor = Color.FromHex(ResourcesReference.APPBAR_ACCENT_COLOR).ToUIColor();
                NavigationController.NavigationBar.TintColor = Color.White.ToUIColor();
            }
        }
    }
}