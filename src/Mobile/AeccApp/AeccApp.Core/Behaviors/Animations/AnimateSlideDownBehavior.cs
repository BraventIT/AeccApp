using AeccApp.Core.Extensions;
using Xamarin.Forms;


namespace AeccApp.Core.Behaviors
{
    public class AnimateSlideDownBehavior : AnimateFoldBehaviorBase
    {
        protected override void Init(bool newValue)
        {
            if (newValue)
            {
                var parentView = AssociateObject.GetParentView();
                if (parentView != null)
                {
                    FoldInPosition = -parentView.Height;
                    AssociateObject.TranslationY = FoldInPosition;
                }
            }
        }

        protected override void ExecuteAnimation(double start, double end, uint runningTime)
        {
            var animation = new Animation(
              d => AssociateObject.TranslationY = d, start, end, Easing.SinOut);

            animation.Commit(AssociateObject, "Unfold", length: runningTime,
              finished: (d, b) =>
              {
                  if (AssociateObject.TranslationY.Equals(FoldInPosition))
                  {
                      AssociateObject.IsVisible = false;
                  }
              });
        }
    }
}
