using FBMS.Core.Constants;
using FBMS.Core.Entities;
using FBMS.Core.Events;
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
    }
}
