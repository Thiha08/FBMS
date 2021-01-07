using System.Collections.Generic;

namespace FBMS.Core.Dtos
{
    public class MemberTransactionTemplateDto : BaseDto
    {
        public int MemberId { get; set; }

        public string MemberName { get; set; }

        public List<TransactionTemplateItemDto> TemplateItems { get; set; }

    }
}
