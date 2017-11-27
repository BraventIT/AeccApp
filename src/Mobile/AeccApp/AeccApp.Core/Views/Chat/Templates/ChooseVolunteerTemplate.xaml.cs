
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AeccApp.Core.Views.Templates
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ChooseVolunteerTemplate : ContentView
	{
		public ChooseVolunteerTemplate()
		{
			InitializeComponent ();

            VolunteersListView.ItemSelected += (sender, e) => ((ListView)sender).SelectedItem = null;

        }
	}
}