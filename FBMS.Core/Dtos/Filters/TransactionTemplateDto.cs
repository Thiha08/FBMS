using System;
using System.Collections.Generic;
using System.Text;

namespace FBMS.Core.Dtos.Filters
{
    public class MemberTransactionTemplateDto : BaseDto
    {
        public int MemberId { get; set; }

        public string MemberName { get; set; }

        public List<TransactionTemplateItemDto> TemplateItems { get; set; }

    }
}
