using FBMS.SharedKernel;
using FBMS.SharedKernel.Interfaces;
using System.Collections.Generic;

namespace FBMS.Core.Entities
{
    public class TransactionTemplate : BaseEntity, IAggregateRoot
    {
        public int MemberId { get; set; }

        public List<TransactionTemplateItem> TemplateItems { get; set; }
    }
}
