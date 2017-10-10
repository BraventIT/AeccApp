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
    public partial class RequestFiltersPopupControl : ContentView
    {
        public RequestFiltersPopupControl()
        {
            InitializeComponent();
        }



        public ICommand ApplyFiltersCommand
        {
            get { return (ICommand)GetValue(ApplyFiltersCommandProperty); }
            set { SetValue(ApplyFiltersCommandProperty, value); }
        }
        public static readonly BindableProperty ApplyFiltersCommandProperty =
          BindableProperty.Create(nameof(ApplyFiltersCommand), typeof(ICommand), typeof(RequestFiltersPopupControl), null);





        public DateTime DateSelected
        {
            get { return (DateTime)GetValue(DateSelectedProperty); }
            set { SetValue(DateSelectedProperty, value); }
        }
        public static readonly BindableProperty DateSelectedProperty =
          BindableProperty.Create(nameof(DateSelected), typeof(DateTime), typeof(RequestFiltersPopupControl), DateTime.Now);





        public TimeSpan TimeSelected
        {
            get { return (TimeSpan)GetValue(TimeSelectedProperty); }
            set { SetValue(TimeSelectedProperty, value); }
        }
        public static readonly BindableProperty TimeSelectedProperty =
          BindableProperty.Create(nameof(TimeSelected), typeof(TimeSpan), typeof(RequestFiltersPopupControl), new TimeSpan(DateTime.Now.Hour,DateTime.Now.Minute,DateTime.Now.Second));












    }
}