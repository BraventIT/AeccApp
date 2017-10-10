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
    public partial class ChatTermsAndConditionsPopupControl : ContentView
    {
        public ChatTermsAndConditionsPopupControl()
        {
            InitializeComponent();
        }

        public ICommand CloseCommand
        {
            get { return (ICommand)GetValue(CloseCommandProperty); }
            set { SetValue(CloseCommandProperty, value); }
        }
        public static readonly BindableProperty CloseCommandProperty =
          BindableProperty.Create(nameof(CloseCommand), typeof(ICommand), typeof(ChatTermsAndConditionsPopupControl), null);

        public ICommand OkCommand
        {
            get { return (ICommand)GetValue(OkCommandProperty); }
            set { SetValue(OkCommandProperty, value); }
        }
        public static readonly BindableProperty OkCommandProperty =
           BindableProperty.Create(nameof(OkCommand), typeof(ICommand), typeof(ChatTermsAndConditionsPopupControl), null);





    }
}