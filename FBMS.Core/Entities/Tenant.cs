using FBMS.SharedKernel;
using FBMS.SharedKernel.Interfaces;

namespace FBMS.Core.Entities
{
    public class Tenant : BaseEntity, IAggregateRoot
    {
        public string HostUserName { get; set; }

        public string HostPassword { get; set; }

        public string ClientUserName { get; set; }

        public string ClientPassword { get; set; }
    }
}
