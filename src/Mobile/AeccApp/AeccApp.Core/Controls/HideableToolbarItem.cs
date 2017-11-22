using AeccApp.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AeccApp.Core.Controls
{
    /// <summary>
    /// Custom control to be able to hide ToobarItem
    /// </summary>

    public class HideableToolbarItem : ToolbarItem
    {
        public HideableToolbarItem() : base()
        {
            InitVisibility();
        }


        private void InitVisibility()
        {
            OnIsVisibleChanged(this, false, IsVisible);
        }

        public bool IsVisible
        {
            get { return (bool)GetValue(IsVisibleProperty); }
            set { SetValue(IsVisibleProperty, value); }
        }

        public static BindableProperty IsVisibleProperty =
            BindableProperty.Create(nameof(IsVisible), typeof(bool), typeof(HideableToolbarItem), true, propertyChanged: OnIsVisibleChanged);

        static void OnIsVisibleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var item = bindable as HideableToolbarItem;

            if (item.Parent == null || newValue == null)
                return;

            var page = item.Parent as Page;

            if (page == null)
                return;

            var items = page.ToolbarItems;

            if ((bool)newValue && !items.Contains(item))
            {
                Device.BeginInvokeOnMainThread(() => items.Add(item));
            }
            else if (!(bool)newValue && items.Contains(item))
            {
                Device.BeginInvokeOnMainThread(() => items.Remove(item));
            }
        }

        public string IconPlatform
        {
            get { return (string)GetValue(IconPlatformProperty); }
            set { SetValue(IconPlatformProperty, value); }
        }

        public static BindableProperty IconPlatformProperty = BindableProperty.Create(
            nameof(IconPlatform),
            typeof(string),
            typeof(HideableToolbarItem),
            null,
            propertyChanged: OnSourcePlatformChanged);

        private static void OnSourcePlatformChanged(BindableObject b, object oldValue, object newValue)
        {
            if (newValue == null)
                return;
            var control = (HideableToolbarItem)b;
            control.SetSourcePlatform((string)newValue);
        }
    }
}
