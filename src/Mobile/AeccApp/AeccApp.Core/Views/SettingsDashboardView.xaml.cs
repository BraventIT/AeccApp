using AeccApp.Core.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AeccApp.Core.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SettingsDashboardView : BaseContentPage
	{
		public SettingsDashboardView ()
		{
			InitializeComponent ();
		}

        protected override void OnAppearing()
        {
            MessagingCenter.Send(new ToolbarMessage(this["SettingsToolbarTitle"]), string.Empty);
            base.OnAppearing();
        }
    }
}