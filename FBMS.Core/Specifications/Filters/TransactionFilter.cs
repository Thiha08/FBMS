using System;
using System.Collections.Generic;
using System.Text;

namespace FBMS.Core.Specifications.Filters
{
    public class TransactionFilter : BaseFilter
    {
        public int? MemberId { get; set; }

        public string UserName { get; set; }
    }
}
