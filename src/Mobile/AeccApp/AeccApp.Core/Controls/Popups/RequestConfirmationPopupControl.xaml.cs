using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AeccApp.Core.Controls
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RequestConfirmationPopupControl : ContentView
	{
		public RequestConfirmationPopupControl ()
		{
			InitializeComponent ();
            
		}

        public ICommand ClosePopupCommand
        {
            get { return (ICommand)GetValue(ClosePopupCommandProperty); }
            set { SetValue(ClosePopupCommandProperty, value); }
        }
        public static readonly BindableProperty ClosePopupCommandProperty =
          BindableProperty.Create(nameof(ClosePopupCommand), typeof(ICommand), typeof(RequestConfirmationPopupControl), null);

        public ICommand SendRequestConfirmationCommand
        {
            get { return (ICommand)GetValue(SendRequestConfirmationCommandProperty); }
            set { SetValue(SendRequestConfirmationCommandProperty, value); }
        }
        public static readonly BindableProperty SendRequestConfirmationCommandProperty =
          BindableProperty.Create(nameof(SendRequestConfirmationCommand), typeof(ICommand), typeof(RequestConfirmationPopupControl), null);



        public string DisplayRequestInfo
        {
            get { return (string)GetValue(DisplayRequestInfoProperty); }
            set { SetValue(DisplayRequestInfoProperty, value); }
        }
        public static readonly BindableProperty DisplayRequestInfoProperty =
          BindableProperty.Create(nameof(DisplayRequestInfo), typeof(string), typeof(RequestConfirmationPopupControl), null);



        public string DisplayDate
        {
            get { return (string)GetValue(DisplayDateProperty); }
            set { SetValue(DisplayDateProperty, value); }
        }
        public static readonly BindableProperty DisplayDateProperty =
          BindableProperty.Create(nameof(DisplayDate), typeof(string), typeof(RequestConfirmationPopupControl), null);

        public string DisplayTime
        {
            get { return (string)GetValue(DisplayTimeProperty); }
            set { SetValue(DisplayTimeProperty, value); }
        }
        public static readonly BindableProperty DisplayTimeProperty =
          BindableProperty.Create(nameof(DisplayTime), typeof(string), typeof(RequestConfirmationPopupControl), null);




    }
}