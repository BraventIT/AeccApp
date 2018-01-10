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
using AeccApp.Core.Messages;

[assembly: ExportRenderer(typeof(TabbedPage), typeof(TabbedPageRenderer))]
namespace AeccApp.iOS.Renderers
{
    public class TabbedPageRenderer : TabbedRenderer
    {
        UIView titleView;
        UIImageView iconView;
        UILabel titleLabel;
        UIImage imageIcon;

        public TabbedPageRenderer()
        {
            TabBar.TintColor = Color.FromHex(ResourcesReference.APPBAR_ACCENT_COLOR).ToUIColor();
            TabBar.UnselectedItemTintColor = Color.FromHex(ResourcesReference.NAV_INACTIVE_COLOR).ToUIColor();
        }

        //public override void ViewDidLayoutSubviews()
        //{
        //    base.ViewDidLayoutSubviews();

        //    if (NavigationController != null)
        //    {
                
        //        NavigationController.NavigationBar.Frame = new CGRect(0, 0, View.Frame.Size.Width, 72.0);
        //        NavigationController.NavigationBar.SetTitleVerticalPositionAdjustment(-5, UIBarMetrics.Default);
        //        var navigationItem = NavigationController.TopViewController.NavigationItem;
        //        var leftNativeButtons = (navigationItem.LeftBarButtonItems ?? new UIBarButtonItem[] { }).ToList();
        //        var rightNativeButtons = (navigationItem.RightBarButtonItems ?? new UIBarButtonItem[] { }).ToList();

        //        leftNativeButtons.ForEach(x => x.ImageInsets = new UIEdgeInsets(-8, 0, 0, 0));
        //        rightNativeButtons.ForEach(x => x.ImageInsets = new UIEdgeInsets(-8, 0, 0, 0));

        //        this.NavigationController.NavigationBar.TintColor = UIColor.White;


        //    }
        //}

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            imageIcon = UIImage.FromBundle("logoHeader.png");
            iconView = new UIImageView(new CGRect(0, 0, 200, 40));
            titleLabel = new UILabel(new CGRect(0, 0, 200, 40));
            titleLabel.TextAlignment = UITextAlignment.Center;

            iconView.ContentMode = UIViewContentMode.ScaleAspectFit;

            titleView = new UIView(new CGRect(0, 0, 200, 40));
            iconView.Frame = titleView.Bounds;

            iconView.Image = imageIcon.ImageWithRenderingMode(UIImageRenderingMode.Automatic);

            MessagingCenter.Subscribe<ToolbarMessage>(this, string.Empty, m =>
            {
                if (NavigationController != null)
                {
                    if (m.ShowLogo)
                    {
                        titleView.AddSubview(iconView);
                        titleLabel.RemoveFromSuperview();
                    }
                    else
                    {
                        iconView.RemoveFromSuperview();
                        titleLabel.Text = m.Title;
                        titleLabel.TextColor = UIColor.White;
                        titleView.AddSubview(titleLabel);
                    }
                    NavigationController.TopViewController.NavigationItem.TitleView = titleView;
                }

            });
        }
    }
}