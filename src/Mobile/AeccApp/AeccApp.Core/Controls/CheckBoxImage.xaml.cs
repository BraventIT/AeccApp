using AeccApp.Core.Extensions;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AeccApp.Core.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CheckBoxImage : ContentView
	{
        const string CHECKED_ID = "checked";

		public CheckBoxImage ()
		{
            InitializeComponent();
		}

        public bool IsChecked
        {
            get { return (bool)GetValue(IsCheckedProperty); }
            set { SetValue(IsCheckedProperty, value); }
        }

        public static BindableProperty IsCheckedProperty = BindableProperty.Create(
            nameof(IsChecked),
            typeof(bool),
            typeof(CheckBoxImage),
            false,
            BindingMode.TwoWay,
             propertyChanged: OnIsCheckChanged
            );

        private static void OnIsCheckChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (CheckBoxImage)bindable;
            bool isCheck = (bool)newValue;

            control.Image.SetSourcePlatform(isCheck ?
                control.SourceChecked : control.SourceUnchecked);
        }

        public string SourceChecked
        {
            get { return (string)GetValue(SourceCheckedProperty); }
            set { SetValue(SourceCheckedProperty, value); }
        }

        public static BindableProperty SourceCheckedProperty = BindableProperty.Create(
            nameof(SourceChecked),
            typeof(string),
            typeof(CheckBoxImage),
            null,
            propertyChanged: OnSourceCheckedChanged);

        private static void OnSourceCheckedChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (CheckBoxImage)bindable;
            if (control.IsChecked)
                control.Image.SetSourcePlatform(control.SourceChecked);
        }

        public string SourceUnchecked
        {
            get { return (string)GetValue(SourceUncheckedProperty); }
            set { SetValue(SourceUncheckedProperty, value); }
        }

        public static BindableProperty SourceUncheckedProperty = BindableProperty.Create(
            nameof(SourceUnchecked),
            typeof(string),
            typeof(CheckBoxImage),
            null,
            propertyChanged: OnSourceUncheckedChanged);

        private static void OnSourceUncheckedChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (CheckBoxImage)bindable;
            if (!control.IsChecked)
                control.Image.SetSourcePlatform(control.SourceUnchecked);
        }

        private void OnImageTapped(object sender, EventArgs args)
        {
            var image = (Image)sender;
            IsChecked = !IsChecked;
        }
    }
}