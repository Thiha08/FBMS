using System;
using System.Collections.Generic;
using System.Text;

namespace FBMS.Core.Dtos
{
    public class MatchDetailDto : BaseDto
    {
        public string BetType { get; set; }

        public bool IsInetBet0 { get; set; }

        public string ModuleTitle { get; set; }

        public string FullTimeId { get; set; }

        public bool IsHomeGive { get; set; }

        public string Home { get; set; }

        public string Away { get; set; }

        public bool IsRun { get; set; }

        public string RunHomeScore { get; set; }

        public string RunAwayScore { get; set; }

        public string GameType3 { get; set; }

        public string BetHdp { get; set; }

        public string BetOdds { get; set; }

        public string ChgErr { get; set; }

        public string BetUrl { get; set; }

        public string MaxLimit { get; set; }

        public string MinLimit { get; set; }

        public string HidOdds { get; set; }

        public string TicketBetType { get; set; }
    }
}
