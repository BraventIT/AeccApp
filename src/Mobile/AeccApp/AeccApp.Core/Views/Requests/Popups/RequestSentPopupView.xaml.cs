using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AeccApp.Core.Views.Popups
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RequestSentPopupView : PopupPage
	{
		public RequestSentPopupView ()
		{
			InitializeComponent ();
		}
	}
}