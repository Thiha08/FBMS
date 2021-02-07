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


        public int MmHdpPricingPrefix { get; set; }

        public int MmHdpPricing { get; set; }

        public int MmHdpPricingSuffix { get; set; } // 0 - A, 1 - H

        public decimal MmHomeAmount { get; set; }

        public decimal MmAwayAmount { get; set; }

        public int MmOuPricingPrefix { get; set; }

        public int MmOuPricing { get; set; }

        public decimal MmOverAmount { get; set; }

        public decimal MmUnderAmount { get; set; }

        public bool IsMm { get; set; }

        public bool IsLive { get; set; }
    }
}
