using FBMS.Core.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace FBMS.Core.Entities
{
    public class TransactionTemplateItem
    {
        public TransactionType TransactionType { get; set; }

        public int AmountPercent { get; set; }

        public bool IsInverse { get; set; }

        public bool IsActive { get; set; }
    }
}
