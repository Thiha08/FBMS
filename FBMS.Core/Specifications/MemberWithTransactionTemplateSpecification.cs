using Ardalis.Specification;
using FBMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace FBMS.Core.Specifications
{
    public class MemberWithTransactionTemplateSpecification : Specification<Member>
    {
        public MemberWithTransactionTemplateSpecification()
        {
            Query.Where(x => x.Status)
                .Include(x => x.TransactionTemplate);
        }
    }
}
