using FBMS.Core.Entities;
using FBMS.SharedKernel;
using System;
using System.Collections.Generic;
using System.Text;

namespace FBMS.Core.Events
{
    public class TransactionTemplateAddedEvent : BaseDomainEvent
    {
        public TransactionTemplate Template { get; set; }

        public TransactionTemplateAddedEvent(TransactionTemplate template)
        {
            Template = template;
        }
    }
}
