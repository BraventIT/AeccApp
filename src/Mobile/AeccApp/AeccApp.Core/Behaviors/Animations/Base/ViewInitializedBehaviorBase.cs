using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AeccApp.Core.Behaviors
{
    public abstract class ViewInitializedBehaviorBase<T> : BaseBindableBehavior<T> where T : VisualElement
    {
        #region Bindable Properties

        public bool ViewIsInitialized
        {
            get { return (bool)GetValue(ViewIsInitializedProperty); }
            set { SetValue(ViewIsInitializedProperty, value); }
        }

        public static readonly BindableProperty ViewIsInitializedProperty =
        BindableProperty.Create(nameof(ViewIsInitialized), typeof(bool), typeof(ViewInitializedBehaviorBase<T>)
            , false, BindingMode.TwoWay
            , propertyChanged: OnViewIsInitializedChanged);

        private static void OnViewIsInitializedChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var thisObj = bindable as ViewInitializedBehaviorBase<T>;
            thisObj?.Init((bool)newValue);
        }
        #endregion

        protected abstract void Init(bool viewIsInitialized);
    }
}
