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
	public partial class RequestChangeRequestPopupControl : ContentView
	{
		public RequestChangeRequestPopupControl ()
		{
			InitializeComponent ();
		}


        public ICommand ClosePopupCommand
        {
            get { return (ICommand)GetValue(ClosePopupCommandProperty); }
            set { SetValue(ClosePopupCommandProperty, value); }
        }
        public static readonly BindableProperty ClosePopupCommandProperty =
          BindableProperty.Create(nameof(ClosePopupCommand), typeof(ICommand), typeof(RequestChangeRequestPopupControl), null);

        public ICommand BackAndChangeTypeCommand
        {
            get { return (ICommand)GetValue(BackAndChangeTypeCommandProperty); }
            set { SetValue(BackAndChangeTypeCommandProperty, value); }
        }
        public static readonly BindableProperty BackAndChangeTypeCommandProperty =
          BindableProperty.Create(nameof(BackAndChangeTypeCommand), typeof(ICommand), typeof(RequestChangeRequestPopupControl), null);












    }
}