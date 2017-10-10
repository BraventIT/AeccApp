using AeccApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AeccApp.Core.Controls
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CounterpartProfilePopupControl: ContentView
    {
		public CounterpartProfilePopupControl()
		{
			InitializeComponent ();
		}

        public UserData Counterpart
        {
            get { return (UserData)GetValue(CounterpartProperty); }
            set { SetValue(CounterpartProperty, value); }
           
        }
        public static readonly BindableProperty CounterpartProperty =
          BindableProperty.Create(nameof(Counterpart), typeof(UserData), typeof(CounterpartProfilePopupControl), null);

    }
}