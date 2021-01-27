using FBMS.Core.Entities;
using FBMS.SharedKernel;
using System;
using System.Collections.Generic;
using System.Text;

namespace FBMS.Core.Events
{
    public class TransactionCompletedEvent : BaseDomainEvent
    {
        public Transaction CompletedTransaction { get; set; }

        public TransactionCompletedEvent(Transaction completedTransaction)
        {
            CompletedTransaction = completedTransaction;
        }
    }
}
