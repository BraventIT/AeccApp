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

[assembly: ExportRenderer(typeof(TabbedPage), typeof(TabbedPageRenderer))]
namespace AeccApp.iOS.Renderers
{
    public class TabbedPageRenderer : TabbedRenderer
    {
        public TabbedPageRenderer()
        {
            TabBar.TintColor = Color.FromHex("#8DD101").ToUIColor();
            TabBar.UnselectedItemTintColor = Color.FromHex("#394A59").ToUIColor();
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

                this.NavigationController.NavigationBar.TintColor = UIColor.White;


            }
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            var image = UIImage.FromBundle("logoHeader.png");
            var imageView = new UIImageView(new CGRect(0, 0, 140, 70));

            imageView.ContentMode = UIViewContentMode.ScaleAspectFit;

            var titleView = new UIView(new CGRect(0, 0, 100, 45));
            imageView.Frame = titleView.Bounds;
            titleView.AddSubview(imageView);

            imageView.Image = image.ImageWithRenderingMode(UIImageRenderingMode.Automatic);

            if (NavigationController != null)
            {
                NavigationController.TopViewController.NavigationItem.TitleView = titleView;
                NavigationController.NavigationBar.BarTintColor = Color.FromHex("#8DD101").ToUIColor();
            }
        }
    }
}