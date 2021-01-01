using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace FBMS.Core.Constants
{
    public enum TransactionType
    {
        [Description("Parlay")]
        Parlay,

        [Description("Home")]
        Home,

        [Description("Away")]
        Away,

        [Description("OVER")]
        Over,

        [Description("Under")]
        Under
    }
}
