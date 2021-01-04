using System;
using System.Collections.Generic;
using System.Text;

namespace FBMS.Core.Ctos.Filters
{
    public class TransactionFilterCto
    {
        public List<int> MemberIds { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
