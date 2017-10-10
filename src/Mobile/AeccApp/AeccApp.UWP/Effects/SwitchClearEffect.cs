using AeccApp.UWP.Effects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: ResolutionGroupName("AeccApp")]
[assembly: ExportEffect(typeof(SwitchClearEffect), "SwitchClearEffect")]
namespace AeccApp.UWP.Effects
{
    public class SwitchClearEffect : PlatformEffect
    {
        ToggleSwitch _control;
        protected override void OnAttached()
        {
            try
            {
                
                _control = Control as ToggleSwitch;
                RemoveText();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Cannot set property on attached control. Error: ", ex.Message);
            }
        }

       

        protected override void OnDetached()
        {
            _control = null;
        }

        private void RemoveText()
        {
            _control.OffContent = string.Empty;
            _control.OnContent = string.Empty;
        }
    }
}
