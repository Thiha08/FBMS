using FBMS.Core.Constants;
using FBMS.SharedKernel;
using FBMS.SharedKernel.Interfaces;

namespace FBMS.Core.Entities
{
    public class TransactionTemplateItem : BaseEntity, IAggregateRoot
    {
        public TransactionType TransactionType { get; set; }

        public int AmountPercent { get; set; }

        public bool IsInverse { get; set; }
    }
}
