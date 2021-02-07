using FBMS.Core.Constants;
using System;
using System.ComponentModel.DataAnnotations;

namespace FBMS.Core.Dtos
{
    public class TransactionDto : BaseDto
    {
        [Display(Name = "Serial No.")]
        public string SerialNumber { get; set; }

        [Display(Name = "Account")]
        public string UserName { get; set; }

        [Display(Name = "Transaction No.")]
        public string TransactionNumber { get; set; }

        [Display(Name = "Transaction Date")]
        public DateTime TransactionDate { get; set; }

        [Display(Name = "League")]
        public string League { get; set; }

        [Display(Name = "Home Team")]
        public string HomeTeam { get; set; }

        [Display(Name = "Away Team")]
        public string AwayTeam { get; set; }

        [Display(Name = "Transaction Type")]
        public TransactionType TransactionType { get; set; }

        [Display(Name = "Pricing")]
        public string Pricing { get; set; }

        [Display(Name = "Amount")]
        public decimal Amount { get; set; }

        public int MemberId { get; set; }

        [Display(Name = "Transaction Type (S)")]
        public TransactionType SubmittedTransactionType { get; set; }

        [Display(Name = "Pricing (S)")]
        public string SubmittedPricing { get; set; }

        [Display(Name = "Amount (S)")]
        public decimal SubmittedAmount { get; set; }

        [Display(Name = "Transaction Date (S)")]
        public DateTime? SubmittedDate { get; set; }

        [Display(Name = "Status")]
        public bool IsSubmitted { get; set; }

        public bool IsDischarged { get; set; }

        public DateTime? DischargedDate { get; set; }

        public int DischargedCount { get; set; }

        public bool IsMmPricing => TransactionNumber.Contains("MM");
    }
}
