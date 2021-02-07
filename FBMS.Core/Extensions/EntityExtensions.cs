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
            return $"JRecPanel.aspx?b=home&oId={match.Oddsid}&odds={match.HomeAmount}&ep={match.HdpPricing}|{match.Ep}";
        }

        public static string GetAwayUrl(this MatchDto match)
        {
            return $"JRecPanel.aspx?b=away&oId={match.Oddsid}&odds={match.AwayAmount}&ep={match.HdpPricing}|{match.Ep}";
        }

        public static string GetOverUrl(this MatchDto match)
        {
            return $"JRecPanel.aspx?b=over&oId={match.Oddsid}&odds={match.OverAmount}&ep={match.OuPricing}";
        }

        public static string GetUnderUrl(this MatchDto match)
        {
            return $"JRecPanel.aspx?b=under&oId={match.Oddsid}&odds={match.UnderAmount}&ep={match.OuPricing}";
        }

        public static string GetMmHomeUrl(this MatchDto match)
        {
            return $"JRecPanel.aspx?b=mmhome&oId={match.Oddsid}&odds={match.MmHomeAmount}";
        }

        public static string GetMmAwayUrl(this MatchDto match)
        {
            return $"JRecPanel.aspx?b=mmaway&oId={match.Oddsid}&odds={match.MmAwayAmount}";
        }

        public static string GetMmOverUrl(this MatchDto match)
        {
            return $"JRecPanel.aspx?b=mmover&oId={match.Oddsid}&odds={match.MmOverAmount}";
        }

        public static string GetMmUnderUrl(this MatchDto match)
        {
            return $"JRecPanel.aspx?b=mmunder&oId={match.Oddsid}&odds={match.MmUnderAmount}";
        }

        public static string GetHtHomeUrl(this MatchDto match)
        {
            return $"JRecPanel.aspx?b=home&oId={match.HtOddsid}&odds={match.HtHomeAmount}&ep={match.HtHdpPricing}|{match.Ep}";
        }

        public static string GetHtAwayUrl(this MatchDto match)
        {
            return $"JRecPanel.aspx?b=away&oId={match.HtOddsid}&odds={match.HtAwayAmount}&ep={match.HtHdpPricing}|{match.Ep}";
        }

        public static string GetHtOverUrl(this MatchDto match)
        {
            return $"JRecPanel.aspx?b=over&oId={match.HtOddsid}&odds={match.HtOverAmount}&ep={match.HtOuPricing}";
        }

        public static string GetHtUnderUrl(this MatchDto match)
        {
            return $"JRecPanel.aspx?b=under&oId={match.HtOddsid}&odds={match.HtUnderAmount}&ep={match.HtOuPricing}";
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
                $"Match : {transaction.HomeTeam} Vs {transaction.AwayTeam}" + Environment.NewLine +
                $"Type : {transaction.TransactionType.ToDescription()}" + Environment.NewLine +
                $"Price : {transaction.Pricing}" + Environment.NewLine +
                $"Is MM Pricing : {(transaction.IsMmPricing ? "YES" : "NO")}" + Environment.NewLine;
        }

        public static string GetInfo(this MatchDto match)
        {
            return
                $"Match : {match.HomeTeam} Vs {match.AwayTeam}" + Environment.NewLine +
                $"Match Date : {match.MatchDate:dd-MM-yyyy HH:mm:ss}" + Environment.NewLine +
                $"Is Live : {(match.IsLive ? "YES" : "NO")}" + Environment.NewLine +
                $"Is Mm : {(match.IsMm ? "YES" : "NO")}" + Environment.NewLine + Environment.NewLine +
                $"HdpPricing : {match.HdpPricing}" + Environment.NewLine +
                $"HomeAmount : {match.HomeAmount}" + Environment.NewLine +
                $"AwayAmount : {match.AwayAmount}" + Environment.NewLine +
                $"OuPricing : {match.OuPricing}" + Environment.NewLine +
                $"OverAmount : {match.OverAmount}" + Environment.NewLine +
                $"UnderAmount : {match.UnderAmount}" + Environment.NewLine + Environment.NewLine +
                $"HtHdpPricing : {match.HtHdpPricing}" + Environment.NewLine +
                $"HtHomeAmount : {match.HtHomeAmount}" + Environment.NewLine +
                $"HtAwayAmount : {match.HtAwayAmount}" + Environment.NewLine +
                $"HtOuPricing : {match.HtOverAmount}" + Environment.NewLine +
                $"HtOverAmount : {match.HtOverAmount}" + Environment.NewLine +
                $"HtUnderAmount : {match.HtUnderAmount}" + Environment.NewLine + Environment.NewLine +
                $"MmHdpPricingPrefix : {match.MmHdpPricingPrefix}" + Environment.NewLine +
                $"MmHdpPricing : {match.MmHdpPricing}" + Environment.NewLine +
                $"MmHdpPricingSuffix : {(match.MmHdpPricingSuffix == 0 ? "A" : "H")}" + Environment.NewLine +
                $"MmHomeAmount : {match.MmHomeAmount}" + Environment.NewLine +
                $"MmAwayAmount : {match.MmAwayAmount}" + Environment.NewLine + Environment.NewLine +
                $"MmOuPricingPrefix : {match.MmOuPricingPrefix}" + Environment.NewLine +
                $"MmOuPricing : {match.MmOuPricing}" + Environment.NewLine +
                $"MmOverAmount : {match.MmOverAmount}" + Environment.NewLine +
                $"MmUnderAmount : {match.MmUnderAmount}" + Environment.NewLine;
        }

        public static string GetInfo(this List<MatchDto> matches)
        {
            var info = "";
            matches.ForEach(x => info += (x.GetInfo() + Environment.NewLine));
            return info;
        }
    }
}
