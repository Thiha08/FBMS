using System;
using System.Collections.Generic;
using System.Text;

namespace FBMS.Core.Dtos
{
    public class MatchDto : BaseDto
    {
        public long Oddsid { get; set; }

        public string League { get; set; }

        public long Soclid { get; set; }

        public string EventKey { get; set; }

        public int Ep { get; set; }

        public string HomeTeam { get; set; }

        public string AwayTeam { get; set; }

        public DateTime MatchDate { get; set; }

        public decimal HdpPricing { get; set; }

        public decimal OuPricing { get; set; }

        public decimal HomeAmount { get; set; }

        public decimal AwayAmount { get; set; }

        public decimal OverAmount { get; set; }

        public decimal UnderAmount { get; set; }

        public long HtOddsid { get; set; }

        public decimal HtHdpPricing { get; set; }

        public decimal HtOuPricing { get; set; }

        public decimal HtHomeAmount { get; set; }

        public decimal HtAwayAmount { get; set; }

        public decimal HtOverAmount { get; set; }

        public decimal HtUnderAmount { get; set; }

        public bool IsLive { get; set; }
    }
}
