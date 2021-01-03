using FBMS.SharedKernel;
using FBMS.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FBMS.Core.Entities
{
    public class Member : BaseEntity, IAggregateRoot
    {
        public string UserName { get; set; }

        public TransactionTemplate TransactionTemplate { get; set; }
    }
}
