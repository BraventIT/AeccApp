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
	public partial class RequestAskForAddressNumberPopupControl : ContentView
	{
		public RequestAskForAddressNumberPopupControl ()
		{
			InitializeComponent ();
		}




        public ICommand ClosePopupCommand
        {
            get { return (ICommand)GetValue(ClosePopupCommandProperty); }
            set { SetValue(ClosePopupCommandProperty, value); }
        }
        public static readonly BindableProperty ClosePopupCommandProperty =
          BindableProperty.Create(nameof(ClosePopupCommand), typeof(ICommand), typeof(RequestAskForAddressNumberPopupControl), null);

        public ICommand ContinueWithoutInputANumberCommand
        {
            get { return (ICommand)GetValue(ContinueWithoutInputANumberCommandProperty); }
            set { SetValue(ContinueWithoutInputANumberCommandProperty, value); }
        }
        public static readonly BindableProperty ContinueWithoutInputANumberCommandProperty=
          BindableProperty.Create(nameof(ContinueWithoutInputANumberCommand), typeof(ICommand), typeof(RequestAskForAddressNumberPopupControl), null);







    }
}