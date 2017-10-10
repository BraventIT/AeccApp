using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AeccApp.Core.Models
{
    public class TabMessage
    {
        public int TabIndex { get; set; }

        public TabMessage(int tabIndex)
        {
            TabIndex = tabIndex;
        }
    }
}
