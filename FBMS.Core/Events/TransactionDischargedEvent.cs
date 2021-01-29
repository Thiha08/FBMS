using FBMS.Core.Entities;
using FBMS.SharedKernel;

namespace FBMS.Core.Events
{
    public class TransactionDischargedEvent : BaseDomainEvent
    {
        public Transaction DischargedTransaction { get; set; }

        public TransactionDischargedEvent(Transaction dischargedTransaction)
        {
            DischargedTransaction = dischargedTransaction;
        }
    }
}
