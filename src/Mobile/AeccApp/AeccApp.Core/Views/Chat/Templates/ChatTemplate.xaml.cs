using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AeccApp.Core.Views.Templates
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ChatTemplate : ContentView
	{
		public ChatTemplate()
		{
			InitializeComponent ();
		}
	}
}