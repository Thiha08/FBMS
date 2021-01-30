using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace FBMS.Core.Constants
{
    public enum TransactionType
    {
        [Description("Parlay")]
        Parlay = 100,

        [Description("Home")]
        Home = 200,

        [Description("Away")]
        Away = 300,

        [Description("Over")]
        Over = 400,

        [Description("Under")]
        Under = 500
    }
}
