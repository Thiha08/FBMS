using FBMS.Core.Attributes;
using FBMS.Core.Constants;
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

        public DateTime Date { get; set; }

        public string League { get; set; }

        public string HomeTeam { get; set; }

        public string AwayTeam { get; set; }

        public TransactionType TransactionType { get; set; }

        public string Pricing { get; set; }

        public decimal Amount { get; set; }

        public int MemberId { get; set; }
    }
}
