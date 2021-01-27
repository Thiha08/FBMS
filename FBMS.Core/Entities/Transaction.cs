using FBMS.Core.Constants;
using FBMS.Core.Events;
using FBMS.SharedKernel;
using FBMS.SharedKernel.Interfaces;
using System;

namespace FBMS.Core.Entities
{
    public class Transaction : BaseEntity, IAggregateRoot
    {
        public string SerialNumber { get; set; }

        public string UserName { get; set; }

        public string TransactionNumber { get; set; }

        public DateTime TransactionDate { get; set; }

        public string League { get; set; }

        public string HomeTeam { get; set; }

        public string AwayTeam { get; set; }

        public TransactionType TransactionType { get; set; }

        public TransactionType SubmittedTransactionType { get; set; }

        public string Pricing { get; set; }

        public string SubmittedPricing { get; set; }

        public decimal Amount { get; set; }

        public decimal SubmittedAmount { get; set; }

        public DateTime? SubmittedDate { get; set; }

        public bool IsSubmitted { get; set; }

        public int MemberId { get; set; }

        public void MarkComplete(string pricing)
        {
            IsSubmitted = true;
            SubmittedPricing = pricing;
            SubmittedDate = DateTime.Now;
            Events.Add(new TransactionCompletedEvent(this));
        }
    }
}
