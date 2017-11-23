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

        public CheckBoxImage()
        {
            InitializeComponent();
            Image?.SetSourcePlatform(SourceUnchecked);
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

            control.Image?.SetSourcePlatform(isCheck ?
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
                control.Image?.SetSourcePlatform(control.SourceChecked);
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
                control.Image?.SetSourcePlatform(control.SourceUnchecked);
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static BindableProperty TextProperty = BindableProperty.Create(
            nameof(Text),
            typeof(string),
            typeof(CheckBoxImage),
            null);

        public Style TextStyle
        {
            get { return (Style)GetValue(TextStyleProperty); }
            set { SetValue(TextStyleProperty, value); }
        }

        public static BindableProperty TextStyleProperty = BindableProperty.Create(
            nameof(TextStyle),
            typeof(Style),
            typeof(CheckBoxImage),
            null);

        public double Spacing
        {
            get { return (double)GetValue(SpacingProperty); }
            set { SetValue(SpacingProperty, value); }
        }

        public static BindableProperty SpacingProperty = BindableProperty.Create(
            nameof(Spacing),
            typeof(double),
            typeof(CheckBoxImage),
            0d);

        public double ImageWidth
        {
            get { return (double)GetValue(ImageWidthProperty); }
            set { SetValue(ImageWidthProperty, value); }
        }

        public static BindableProperty ImageWidthProperty = BindableProperty.Create(
            nameof(ImageWidth),
            typeof(double),
            typeof(CheckBoxImage),
            20d);

        public double ImageHeight
        {
            get { return (double)GetValue(ImageHeightProperty); }
            set { SetValue(ImageHeightProperty, value); }
        }

        public static BindableProperty ImageHeightProperty = BindableProperty.Create(
            nameof(ImageHeight),
            typeof(double),
            typeof(CheckBoxImage),
            20d);

        private void OnImageTapped(object sender, EventArgs args)
        {
            IsChecked = !IsChecked;
        }
    }
}