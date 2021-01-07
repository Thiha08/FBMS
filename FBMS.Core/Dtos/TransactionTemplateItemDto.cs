using FBMS.Core.Constants;

namespace FBMS.Core.Dtos
{
    public class TransactionTemplateItemDto : BaseDto
    {
        public string Name { get; set; }

        public TransactionType TransactionType { get; set; }

        public int AmountPercent { get; set; }

        public bool IsInverse { get; set; }

        public bool Status { get; set; }
    }
}
