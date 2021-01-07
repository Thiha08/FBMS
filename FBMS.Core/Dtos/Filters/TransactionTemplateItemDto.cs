using FBMS.Core.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace FBMS.Core.Dtos.Filters
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
