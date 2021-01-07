using FBMS.Core.Constants;
using FBMS.Core.Entities;
using System.Collections.Generic;

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
                }); ;
            }
            return transactionTemplate;
        }
    }
}
