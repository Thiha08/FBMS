using System;
using System.Collections.Generic;
using System.Text;

namespace FBMS.Core.Dtos.Filters
{
    public class TransactionFilterDto : BaseFilterDto
    {
        public int? MemberId { get; set; }

        public string UserName { get; set; }

        public bool? IsSubmitted { get; set; }

        public bool? IsDischarged { get; set; }

        public int? MaxDischargedCount { get; set; }

        public bool IsDateRangeFilter { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}
