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
    public partial class ChatLeaseConversationPopupControl : ContentView
    {
        public ChatLeaseConversationPopupControl()
        {
            InitializeComponent();
        }

        public ICommand YesCommand
        {
            get { return (ICommand)GetValue(YesCommandProperty); }
            set { SetValue(YesCommandProperty, value); }
        }

        public static readonly BindableProperty YesCommandProperty =
            BindableProperty.Create(nameof(YesCommand), typeof(ICommand), typeof(ChatLeaseConversationPopupControl), null);

        public ICommand NoCommand
        {
            get { return (ICommand)GetValue(NoCommandProperty); }
            set { SetValue(NoCommandProperty, value); }
        }

        public static readonly BindableProperty NoCommandProperty =
            BindableProperty.Create(nameof(NoCommand), typeof(ICommand), typeof(ChatLeaseConversationPopupControl), null);

    }
}