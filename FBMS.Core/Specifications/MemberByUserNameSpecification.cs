using Ardalis.Specification;
using FBMS.Core.Entities;

namespace FBMS.Core.Specifications
{
    public class MemberByUserNameSpecification : Specification<Member>
    {
        public MemberByUserNameSpecification(string userName)
        {
            Query.Where(x => x.UserName == userName)
                 .OrderBy(x => x.UserName);
        }
    }
}
