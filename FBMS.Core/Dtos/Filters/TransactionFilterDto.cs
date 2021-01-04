using System;
using System.Collections.Generic;
using System.Text;

namespace FBMS.Core.Dtos.Filters
{
    public class TransactionFilterDto : BaseFilterDto
    {
        public int? MemberId { get; set; }

        public string UserName { get; set; }
    }
}
