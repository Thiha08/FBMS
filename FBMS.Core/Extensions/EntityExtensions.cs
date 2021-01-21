using FBMS.Core.Constants;
using FBMS.Core.Dtos;
using FBMS.Core.Entities;
using FBMS.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FBMS.Core.Extensions
{
    public static class EntityExtensions
    {
        public static TransactionTemplate GetDefaultTransactionTemplate(this TransactionTemplate transactionTemplate)
        {
            var transactionTypes = EnumerationUtility.GetValues<TransactionType>();
            transactionTemplate.TemplateItems = new List<TransactionTemplateItem>();
            foreach (var transactionType in transactionTypes)
            {
                transactionTemplate.TemplateItems.Add(new TransactionTemplateItem
                {
                    Name = transactionType.ToDescription(),
                    TransactionType = transactionType,
                    AmountPercent = 100,
                    IsInverse = true
                });
            }
            transactionTemplate.Events.Add(new TransactionTemplateAddedEvent(transactionTemplate));
            return transactionTemplate;
        }

        public static Transaction ApplyTransactionTemplate(this TransactionTemplate transactionTemplate, Transaction transaction)
        {
            var templateItem = transactionTemplate.TemplateItems.Where(x => x.TransactionType == transaction.TransactionType).FirstOrDefault();
            if (templateItem.IsInverse)
            {
                transaction.SubmittedTransactionType = transaction.TransactionType.InvertTransactionType();
            }
            else
            {
                transaction.SubmittedTransactionType = transaction.TransactionType;
            }
            if (templateItem.AmountPercent > 0)
            {
                transaction.SubmittedAmount = decimal.Round((templateItem.AmountPercent * transaction.Amount) / 100);
            }
            transaction.Status = templateItem.Status;
            return transaction;
        }

        public static TransactionType InvertTransactionType(this TransactionType transactionType)
        {
            if (transactionType == TransactionType.Home)
                return TransactionType.Away;
            else if (transactionType == TransactionType.Away)
                return TransactionType.Home;
            else if (transactionType == TransactionType.Over)
                return TransactionType.Under;
            else if (transactionType == TransactionType.Under)
                return TransactionType.Over;
            else
                return transactionType;
        }

        public static string GetHomeUrl(this MatchDto match)
        {
            return $"JRecPanel.aspx?b=home&oId={match.Oddsid}&odds={match.HomeAmount}&ep={match.FtHdpPricing}|{match.Ep}";
        }

        public static string GetAwayUrl(this MatchDto match)
        {
            return $"JRecPanel.aspx?b=away&oId={match.Oddsid}&odds={match.AwayAmount}&ep={match.FtHdpPricing}|{match.Ep}";
        }

        public static string GetOverUrl(this MatchDto match)
        {
            return $"JRecPanel.aspx?b=over&oId={match.Oddsid}&odds={match.OverAmount}&ep={match.FtOuPricing}";
        }

        public static string GetUnderUrl(this MatchDto match)
        {
            return $"JRecPanel.aspx?b=under&oId={match.Oddsid}&odds={match.UnderAmount}&ep={match.FtOuPricing}";
        }

        public static string GetBetUrl(this MatchBetDto match)
        {
            return $"{match.BetUrl}&amt={match.Stack}&isBetterOdds=true";
        }

        public static string GetInfo(this TransactionDto transaction)
        {
            return
                $"Account : {transaction.UserName}" + Environment.NewLine +
                $"Transaction Number : {transaction.TransactionNumber}" + Environment.NewLine +
                $"Transaction Date : {transaction.TransactionDate:dd-MM-yyyy HH:mm:ss}" + Environment.NewLine +
                $"Match : {transaction.HomeTeam} Vs {transaction.AwayTeam}";
        }
    }
}
