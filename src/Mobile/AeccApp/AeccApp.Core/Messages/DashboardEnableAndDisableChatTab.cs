using System;
using System.Collections.Generic;
using System.Text;

namespace AeccApp.Core.Messages
{
    class DashboardEnableAndDisableChatTab
    {
        public bool Message { get; set; }

        public DashboardEnableAndDisableChatTab(bool isEnabled)
        {
            Message = isEnabled;
        }
    }
}
