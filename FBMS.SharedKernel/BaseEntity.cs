using System;
using System.Collections.Generic;
using System.Text;

namespace FBMS.SharedKernel
{
    // This can be modified to BaseEntity<TId> to support multiple key types (e.g. Guid)
    public abstract class BaseEntity
    {
        public int Id { get; set; }

        public DateTime? DateCreated { get; set; }

        public DateTime? DateUpdated { get; set; }

        public bool Status { get; set; }

        public List<BaseDomainEvent> Events = new List<BaseDomainEvent>();
    }
}
