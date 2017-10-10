using AeccApp.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AeccApp.Core.Behaviors
{
    public class AnimateSlideInBehavior : AnimateFoldBehaviorBase
    {
        protected override void Init(bool newValue)
        {
            if (newValue)
            {
                var parentView = AssociateObject.GetParentView();
                if (parentView != null)
                {
                    FoldInPosition = -parentView.Width;
                    AssociateObject.TranslationX = FoldInPosition;
                }
            }
        }

        protected override void ExecuteAnimation(double start, double end, uint runningTime)
        {
            var animation = new Animation(
              d => AssociateObject.TranslationX = d, start, end, Easing.SinOut);

            animation.Commit(AssociateObject, "Unfold", length: runningTime,
              finished: (d, b) =>
              {
                  if (AssociateObject.TranslationX.Equals(FoldInPosition))
                  {
                      AssociateObject.IsVisible = false;
                  }
              });
        }
    }
}
