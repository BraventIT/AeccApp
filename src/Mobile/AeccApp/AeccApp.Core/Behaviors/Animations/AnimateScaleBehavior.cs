using Xamarin.Forms;

namespace AeccApp.Core.Behaviors
{
    public class AnimateScaleBehavior : AnimateFoldBehaviorBase
    {
        protected override void Init(bool newValue)
        {
            if (newValue)
            {
                FoldOutPosition = 1;
                FoldInPosition = 0;
                AssociateObject.Scale = FoldInPosition;
            }
        }

        protected override void ExecuteAnimation(double start, double end, uint runningTime)
        {
            var animation = new Animation(
              d => AssociateObject.Scale = d, start, end, Easing.SinOut);

            animation.Commit(AssociateObject, "Unfold", length: runningTime,
              finished: (d, b) =>
              {
                  if (AssociateObject.Scale.Equals(FoldInPosition))
                  {
                      AssociateObject.IsVisible = false;
                  }
              });
        }
    }
}
