using System;
using System.Collections.Generic;
using System.Text;

namespace AeccApp.Core.Messages
{
    public class DashboardHideRequestsTabMessage
    {
        public TabsEnum Message { get; set; }

        public DashboardHideRequestsTabMessage(TabsEnum tab)
        {
            Message = tab;
        }
    }
}
