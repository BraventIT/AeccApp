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
	public partial class RequestSentPopupControl : ContentView
	{
		public RequestSentPopupControl ()
		{
			InitializeComponent ();
		}

        
               public ICommand CloseRequestSentPopupCommand
        {
            get { return (ICommand)GetValue(CloseRequestSentPopupCommandProperty); }
            set { SetValue(CloseRequestSentPopupCommandProperty, value); }
        }

        public static readonly BindableProperty CloseRequestSentPopupCommandProperty =
            BindableProperty.Create(nameof(CloseRequestSentPopupCommand), typeof(ICommand), typeof(RequestSentPopupControl), null);









    }
}