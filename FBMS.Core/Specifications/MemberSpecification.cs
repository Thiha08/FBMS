using Ardalis.Specification;
using FBMS.Core.Entities;
using FBMS.Core.Specifications.Filters;

namespace FBMS.Core.Specifications
{
    public class MemberSpecification : Specification<Member>
    {
        public MemberSpecification(MemberFilter filter)
        {
            Query.OrderBy(x => x.UserName);

            if (filter.LoadChildren)
                Query.Include(x => x.TransactionTemplate)
                    .ThenInclude(x => x.TemplateItems);

            if (filter.IsPagingEnabled)
                Query.Skip(PaginationHelper.CalculateSkip(filter))
                     .Take(PaginationHelper.CalculateTake(filter));

            if (!string.IsNullOrEmpty(filter.UserName))
                Query.Where(x => x.UserName == filter.UserName);

            if (filter.Status.HasValue)
                Query.Where(x => x.Status == filter.Status);
        }
    }
}
