using FBMS.Core.Constants;
using System;
using System.ComponentModel.DataAnnotations;

namespace FBMS.Core.Dtos
{
    public class TransactionDto : BaseDto
    {
        public string SerialNumber { get; set; }

        [Display(Name = "Account")]
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
