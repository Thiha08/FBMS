using System;
using System.Collections.Generic;
using System.Text;

namespace FBMS.Core.Specifications.Filters
{
    public class MemberFilter : BaseFilter
    {
        public string UserName { get; set; }

        public bool? Status { get; set; }
    }
}
