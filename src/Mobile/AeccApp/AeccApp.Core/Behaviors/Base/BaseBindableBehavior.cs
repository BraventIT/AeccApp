using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AeccApp.Core.Behaviors
{
    public abstract class BaseBindableBehavior<T> :Behavior<T> where T: BindableObject
    {
        protected T AssociateObject { get; private set; }

        protected override void OnAttachedTo(T visualElement)
        {
            base.OnAttachedTo(visualElement);

            AssociateObject = visualElement;

            if (visualElement.BindingContext!=null)
            {
                BindingContext = visualElement.BindingContext;
            }

            visualElement.BindingContextChanged += OnBindingContextChanged;
        }

        protected override void OnDetachingFrom(T visualElement)
        {
            base.OnDetachingFrom(visualElement);

            visualElement.BindingContextChanged -= OnBindingContextChanged;
        }

        private void OnBindingContextChanged(object sender, EventArgs e)
        {
            OnBindingContextChanged();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            BindingContext = AssociateObject.BindingContext;
        }
    }
}
