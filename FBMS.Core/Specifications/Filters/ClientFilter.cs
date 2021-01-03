using System;
using System.Collections.Generic;
using System.Text;

namespace FBMS.Core.Specifications.Filters
{
    public class ClientFilter : BaseFilter
    {
        public string Account { get; set; }

        public bool? Status { get; set; }
    }
}
