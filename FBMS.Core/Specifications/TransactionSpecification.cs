using Ardalis.Specification;
using FBMS.Core.Entities;
using FBMS.Core.Specifications.Filters;

namespace FBMS.Core.Specifications
{
    public class TransactionSpecification : Specification<Transaction>
    {
        public TransactionSpecification(TransactionFilter filter)
        {
            Query.OrderBy(x => x.UserName);

            if (filter.IsPagingEnabled)
                Query.Skip(PaginationHelper.CalculateSkip(filter))
                     .Take(PaginationHelper.CalculateTake(filter));

            if (!string.IsNullOrEmpty(filter.UserName))
                Query.Where(x => x.UserName == filter.UserName);

            if (filter.MemberId.HasValue)
                Query.Where(x => x.MemberId == filter.MemberId);

            if (filter.IsSubmitted.HasValue)
                Query.Where(x => x.IsSubmitted == filter.IsSubmitted);

            if (filter.IsDischarged.HasValue)
                Query.Where(x => x.IsDischarged == filter.IsDischarged);
        }
    }
}
