using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AeccApp.Core.Views.Templates
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChatTemplate : ContentView
    {
        public ChatTemplate()
        {
            InitializeComponent();

            Entry.Effects.Add(Effect.Resolve("EntryEffects.EntryEffect"));
            MessageList.ItemSelected += (sender, e) => ((ListView)sender).SelectedItem = null;
        }

        public Command CounterpartClickCommand
        {
            get { return (Command)GetValue(CounterpartClickCommandProperty); }
            set { SetValue(CounterpartClickCommandProperty, value); }
        }

        public static BindableProperty CounterpartClickCommandProperty =
            BindableProperty.Create(
                nameof(CounterpartClickCommand),
                typeof(Command),
                typeof(ChatTemplate),
                null);
    }
}