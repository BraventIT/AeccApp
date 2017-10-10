using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace AeccApp.Core.Controls
{
   public class NonScrollableListView : ListView
    {
        /// <summary>
        /// Custom control deactivating scroll in ListView (used on HomeView.xaml for news list)
        /// </summary>

        public NonScrollableListView()
            : base(ListViewCachingStrategy.RecycleElement)
        {
            ItemTapped += ListView_ItemTapped;

            if (Device.OS == TargetPlatform.Windows || Device.OS == TargetPlatform.WinPhone)
                BackgroundColor = Color.White;
            
        }

        #region ItemSelectedCommand Bindable Property

        public static BindableProperty ItemSelectedCommandProperty =
            BindableProperty.Create<NonScrollableListView, ICommand>(x => x.ItemSelectedCommand, null);

        public ICommand ItemSelectedCommand
        {
            get { return (ICommand)GetValue(ItemSelectedCommandProperty); }
            set { SetValue(ItemSelectedCommandProperty, value); }
        }

        #endregion

        void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item != null && ItemSelectedCommand != null && ItemSelectedCommand.CanExecute(e.Item))
            {
                ItemSelectedCommand.Execute(e.Item);
            }

            SelectedItem = null;
        }
    }
}
