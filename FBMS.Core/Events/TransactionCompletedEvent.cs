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

        public string Message { get; set; }

        public TransactionCompletedEvent(Transaction completedTransaction, string message)
        {
            CompletedTransaction = completedTransaction;
            Message = message;
        }
    }
}
