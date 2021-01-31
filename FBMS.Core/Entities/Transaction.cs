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

        public bool IsDischarged { get; set; }

        public DateTime? DischargedDate { get; set; }

        public int DischargedCount { get; set; }

        public int MemberId { get; set; }

        public void MarkComplete(string pricing, string message)
        {
            IsSubmitted = true;
            SubmittedPricing = pricing;
            SubmittedDate = DateTime.UtcNow;
            Events.Add(new TransactionCompletedEvent(this, message));
        }

        public void MarkDischarge(string message)
        {
            IsDischarged = true;
            DischargedCount++;
            DischargedDate = DateTime.UtcNow;
            Events.Add(new TransactionDischargedEvent(this, message));
        }
    }
}
