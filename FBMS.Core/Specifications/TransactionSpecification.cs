using Ardalis.Specification;
using FBMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace FBMS.Core.Specifications
{
    public class TransactionSpecification : Specification<Transaction>
    {
        public TransactionSpecification(int clientId)
        {
            Query.Where(x => x.ClientId == clientId);
        }
    }
}
