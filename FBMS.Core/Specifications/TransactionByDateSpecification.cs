using Ardalis.Specification;
using FBMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace FBMS.Core.Specifications
{
    public class TransactionByDateSpecification : Specification<Transaction>
    {
        public TransactionByDateSpecification()
        {
            DateTime startDateTime = DateTime.UtcNow.Date.AddDays(-1);
            DateTime endDateTime = DateTime.UtcNow.Date.AddDays(1).AddTicks(-1);

            Query.Where(item => item.DateCreated >= startDateTime && item.DateCreated <= endDateTime);
        }
    }
}
