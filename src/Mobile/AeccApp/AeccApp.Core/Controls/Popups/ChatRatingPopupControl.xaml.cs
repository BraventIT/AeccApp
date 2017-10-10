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
	public partial class ChatRatingPopupControl : ContentView
	{
		public ChatRatingPopupControl ()
		{
			InitializeComponent ();
		}


        public ICommand RatingCommand
        {
            get { return (ICommand)GetValue(RatingCommandProperty); }
            set { SetValue(RatingCommandProperty, value); }
        }

        public static readonly BindableProperty RatingCommandProperty =
            BindableProperty.Create(nameof(RatingCommand), typeof(ICommand), typeof(ChatRatingPopupControl), null);

        

    }
}