using Ardalis.Specification;
using FBMS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace FBMS.Core.Specifications
{
    public class AcitveMembersSpecification : Specification<Member>
    {
        public AcitveMembersSpecification(List<string> userNames)
        {
            Query.Where(x => x.Status && userNames.Contains(x.UserName));
        }
    }
}
