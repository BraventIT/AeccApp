using AeccApp.Core.Models;
using AeccApp.Core.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace AeccApp.Core.ViewModels.Popups
{
   public class ChatCounterpartProfilePopupViewModel : ViewModelBase
    {
      
        private UserData _counterpart;

        public UserData Counterpart
        {
            get { return _counterpart; }
            set { _counterpart = value; }
        }


    }
}
