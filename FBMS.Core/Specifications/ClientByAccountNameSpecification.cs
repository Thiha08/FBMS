using Ardalis.Specification;
using FBMS.Core.Entities;

namespace FBMS.Core.Specifications
{
    public class ClientByAccountNameSpecification : Specification<Client>
    {
        public ClientByAccountNameSpecification(string accountName)
        {
            Query.Where(x => x.Account == accountName)
                 .OrderBy(x => x.Account);
        }
    }
}
