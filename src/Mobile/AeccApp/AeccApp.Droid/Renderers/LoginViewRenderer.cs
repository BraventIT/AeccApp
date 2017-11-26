using AeccApp.Core.Views;
using AeccApp.Droid.Renderers;
using Android.App;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(LoginView), typeof(LoginViewRenderer))]
namespace AeccApp.Droid.Renderers
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
