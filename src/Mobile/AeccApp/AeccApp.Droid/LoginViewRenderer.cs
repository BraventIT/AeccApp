
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using Android.App;
using AeccApp.Core.Views;
using AeccApp.Droid;

[assembly: ExportRenderer(typeof(LoginView), typeof(LoginViewRenderer))]
namespace AeccApp.Droid
{
	class LoginViewRenderer : PageRenderer
	{
		LoginView page;

		protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
		{
			base.OnElementChanged(e);
			page = e.NewElement as LoginView;
			var activity = this.Context as Activity;
		}

	}
}
