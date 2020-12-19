using FBMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBMS.Web.ApiModels
{
    // Note: doesn't expose events or behavior
    public class TransactionDTO
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string SerialNumber { get; set; }
        public string Account { get; set; }
        public string TransactionNumber { get; set; }
        public string Date { get; set; }
        public string League { get; set; }
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public string PricingType { get; set; }
        public string Pricing { get; set; }
        public string Amount { get; set; }

        public static TransactionDTO FromTransaction(Transaction item)
        {
            return new TransactionDTO()
            {
                Id = item.Id,
                ClientId = item.ClientId,
                SerialNumber = item.SerialNumber,
                Account = item.Account,
                TransactionNumber = item.TransactionNumber,
                Date = item.Date,
                League = item.League,
                HomeTeam = item.HomeTeam,
                AwayTeam = item.AwayTeam,
                PricingType = item.PricingType,
                Pricing = item.Pricing,
                Amount = item.Amount
            };
        }
    }
}
