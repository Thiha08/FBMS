using System;
using System.Collections.Generic;
using System.Text;

namespace FBMS.Core.Constants
{
    public static class TransactionResponseStatus
    {
        public static string NoBet { get { return "NOBET"; } }

        public static string OddChanged { get { return "Odds has changed to"; } }

        public static string OddUnavailable { get { return "Odds is not available"; } }

        public static string MatchNotFound { get { return "Cannot find related Match Detail"; } }

        public static string StatusAccepted { get { return "BET SUCCESSFULLY"; } }

        public static string StatusWaiting { get { return "LIVE BETTING"; } }
    }
}
