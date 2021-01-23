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
            DateTime startDateTime = DateTime.Today.AddDays(-2); //Yesterday at 00:00:00
            DateTime endDateTime = DateTime.Today.AddDays(1).AddTicks(-1); //Today at 23:59:59

            Query.Where(item => item.DateCreated >= startDateTime && item.DateCreated <= endDateTime);
        }
    }
}
