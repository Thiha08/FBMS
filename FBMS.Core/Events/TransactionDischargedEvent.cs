using FBMS.Core.Entities;
using FBMS.SharedKernel;

namespace FBMS.Core.Events
{
    public class TransactionDischargedEvent : BaseDomainEvent
    {
        public Transaction DischargedTransaction { get; set; }

        public string Message { get; set; }

        public TransactionDischargedEvent(Transaction dischargedTransaction, string message)
        {
            DischargedTransaction = dischargedTransaction;
            Message = message;
        }
    }
}
