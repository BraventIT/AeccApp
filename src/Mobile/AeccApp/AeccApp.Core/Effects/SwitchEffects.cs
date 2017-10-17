using System;
using System.Linq;
using Xamarin.Forms;

namespace AeccApp.Core.Effects
{
    public class SwitchEffects
    {
        public static bool GetClear(BindableObject view)
        {
            return (bool)view.GetValue(ClearProperty);
        }

        public static void SetClear(BindableObject view, bool clear)
        {
            view.SetValue(ClearProperty, clear);
        }

        public static readonly BindableProperty ClearProperty =
        BindableProperty.CreateAttached("Clear", typeof(bool), typeof(SwitchEffects), false, propertyChanged: OnClearChanged);

        private static void OnClearChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = bindable as View;
            if (view == null)
                return;

            var hasClear = (bool)newValue;
            if (hasClear)
            {
                view.Effects.Add(new SwitchClearEffect());
            }
            else
            {
                var toRemove = view.Effects.FirstOrDefault(e => e is SwitchClearEffect);
                if (toRemove != null)
                    view.Effects.Remove(toRemove);
            }
        }
    }

    public class SwitchClearEffect : RoutingEffect
    {
        /// <summary>
        /// Clears UWP toggle text
        /// </summary>
        public SwitchClearEffect() : base("AeccApp.SwitchClearEffect") { }
    }
}
