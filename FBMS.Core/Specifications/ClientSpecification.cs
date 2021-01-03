using Ardalis.Specification;
using FBMS.Core.Entities;
using FBMS.Core.Specifications.Filters;

namespace FBMS.Core.Specifications
{
    public class ClientSpecification : Specification<Client>
    {
        public ClientSpecification(ClientFilter filter)
        {
            Query.OrderBy(x => x.Account);

            if (filter.IsPagingEnabled)
                Query.Skip(PaginationHelper.CalculateSkip(filter))
                     .Take(PaginationHelper.CalculateTake(filter));

            if (!string.IsNullOrEmpty(filter.Account))
                Query.Where(x => x.Account == filter.Account);

            if (filter.Status.HasValue)
                Query.Where(x => x.Status == filter.Status);
        }
    }
}
