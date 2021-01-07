using Ardalis.Specification;
using FBMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace FBMS.Core.Specifications
{
    public class TransactionTemplateSpecification : Specification<TransactionTemplate>
    {
        public TransactionTemplateSpecification(int id = 0)
        {
            if (id > 0)
                Query.Where(x => x.Id == id);

            Query.Where(x => x.Status)
                .Include(x => x.TemplateItems);
        }
    }
}
