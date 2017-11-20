using AeccApp.Core.Models;
using AeccApp.Core.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AeccApp.Core.ViewModels
{
    public class ChatCounterpartProfileViewModel : ViewModelBase
    {

        public override Task InitializeAsync(object navigationData)
        {
            Counterpart = (UserData)navigationData;

            return Task.CompletedTask;
        }

        private UserData _counterpart;

        public UserData Counterpart
        {
            get { return _counterpart; }
            set { Set(ref _counterpart, value); }
        }
    }
}
