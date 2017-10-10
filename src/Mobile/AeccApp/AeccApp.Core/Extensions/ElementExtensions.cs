using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AeccApp.Core.Extensions
{
    public static class ElementExtensions
    {
        public static VisualElement GetParentView(this Element element)
        {
            var parent = element;
            VisualElement parentView = null;
            if (parent != null)
            {
                do
                {
                    parent = parent.Parent;
                    parentView = parent as VisualElement;
                } while (parentView?.Width <= 0 && parent.Parent != null);
            }

            return parentView;
        }
    }
}
