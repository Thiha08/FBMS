using Ardalis.Specification;
using FBMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace FBMS.Core.Specifications
{
    public class MemberWithTransactionTemplateSpecification : Specification<Member>
    {
        public MemberWithTransactionTemplateSpecification(int? id, bool? status)
        {
            if (id > 0)
                Query.Where(x => x.Id == id);

            if (status.HasValue)
                Query.Where(x => x.Status == status);

            Query.Include(x => x.TransactionTemplate)
                 .ThenInclude(x => x.TemplateItems);
        }
    }
}
