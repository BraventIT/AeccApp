using AeccApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AeccApp.Core.Messages
{
    public enum TabsEnum
    {
        Home = 0, Chat = 1, Requests = 2, Profile = 3
    }

    /// <summary>
    /// Listens to messages to properly switch tab with native event
    /// </summary>
    public class DashboardTabMessage
    {
        public TabsEnum Message { get; set; }

        public DashboardTabMessage(TabsEnum tab)
        {
            Message = tab;
        }
    }
}
