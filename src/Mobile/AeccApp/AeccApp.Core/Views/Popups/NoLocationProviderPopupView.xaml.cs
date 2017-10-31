using Rg.Plugins.Popup.Pages;
using Xamarin.Forms.Xaml;

namespace AeccApp.Core.Views.Popups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NoLocationProviderPopupView : PopupPage
	{
		public NoLocationProviderPopupView ()
		{
			InitializeComponent ();
		}
	}
}