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
	public partial class RequestDateAndTimePopupControl : ContentView
	{
		public RequestDateAndTimePopupControl ()
		{
			InitializeComponent ();
            
		}



        public ICommand ClosePopupCommand
        {
            get { return (ICommand)GetValue(ClosePopupCommandProperty); }
            set { SetValue(ClosePopupCommandProperty, value); }
        }
        public static readonly BindableProperty ClosePopupCommandProperty =
          BindableProperty.Create(nameof(ClosePopupCommand), typeof(ICommand), typeof(RequestDateAndTimePopupControl), null);




        public ICommand ApplyDateAndTimeCommand
        {
            get { return (ICommand)GetValue(ApplyDateAndTimeCommandProperty); }
            set { SetValue(ApplyDateAndTimeCommandProperty, value); }
        }
        public static readonly BindableProperty ApplyDateAndTimeCommandProperty =
          BindableProperty.Create(nameof(ApplyDateAndTimeCommand), typeof(ICommand), typeof(RequestDateAndTimePopupControl), null);




        public DateTime DateSelected
        {
            get { return (DateTime)GetValue(DateSelectedProperty); }
            set { SetValue(DateSelectedProperty, value); }
        }
        public static readonly BindableProperty DateSelectedProperty =
          BindableProperty.Create(nameof(DateSelected), typeof(DateTime), typeof(RequestDateAndTimePopupControl), DateTime.Now);





        public TimeSpan TimeSelected
        {
            get { return (TimeSpan)GetValue(TimeSelectedProperty); }
            set { SetValue(TimeSelectedProperty, value); }
        }
        public static readonly BindableProperty TimeSelectedProperty =
          BindableProperty.Create(nameof(TimeSelected), typeof(TimeSpan), typeof(RequestDateAndTimePopupControl), new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second));








    }
}