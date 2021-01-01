using FBMS.SharedKernel;
using FBMS.SharedKernel.Interfaces;
using System.Collections.Generic;

namespace FBMS.Core.Entities
{
    public class TransactionTemplate : BaseEntity, IAggregateRoot
    {
        public List<TransactionTemplateItem> Items { get; set; }

        public int ClientId { get; set; }

        public Client Client { get; set; }
    }
}
