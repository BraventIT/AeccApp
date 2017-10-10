using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AeccApp.Core.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChatFiltersPopupControl : ContentView
    {
        public ChatFiltersPopupControl()
        {
            InitializeComponent();
        }
        public int MaximumAge
        {
            get { return (int)GetValue(MaximumAgeProperty); }
            set { SetValue(MaximumAgeProperty, value); }
        }
        public static readonly BindableProperty MaximumAgeProperty =
          BindableProperty.Create(nameof(MaximumAge), typeof(int), typeof(ChatFiltersPopupControl), 80);

        public int MinimumAge
        {
            get { return (int)GetValue(MinimumAgeProperty); }
            set { SetValue(MinimumAgeProperty, value); }
        }
        public static readonly BindableProperty MinimumAgeProperty =
          BindableProperty.Create(nameof(MinimumAge), typeof(int), typeof(ChatFiltersPopupControl), 18);


        public ICommand ApplyFiltersCommand
        {
            get { return (ICommand)GetValue(ApplyFiltersCommandProperty); }
            set { SetValue(ApplyFiltersCommandProperty, value); }
        }
        public static readonly BindableProperty ApplyFiltersCommandProperty =
          BindableProperty.Create(nameof(ApplyFiltersCommand), typeof(ICommand), typeof(ChatFiltersPopupControl), null);

        public ICommand SelectTimeCommand
        {
            get { return (ICommand)GetValue(SelectTimeCommandProperty); }
            set { SetValue(SelectTimeCommandProperty, value); }
        }
        public static readonly BindableProperty SelectTimeCommandProperty =
          BindableProperty.Create(nameof(SelectTimeCommand), typeof(ICommand), typeof(ChatFiltersPopupControl), null);


        public ICommand SelectDateCommand
        {
            get { return (ICommand)GetValue(SelectDateCommandProperty); }
            set { SetValue(SelectDateCommandProperty, value); }
        }
        public static readonly BindableProperty SelectDateCommandProperty =
          BindableProperty.Create(nameof(SelectDateCommand), typeof(ICommand), typeof(ChatFiltersPopupControl), null);






    }
}