using FBMS.SharedKernel;
using FBMS.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FBMS.Core.Entities
{
    public class Transaction : BaseEntity, IAggregateRoot
    {
        public DateTime Date { get; set; }

        public string Event { get; set; }

        public string Detail { get; set; }

        public decimal Amount { get; set; }

        public int ClientId { get; set; }

        public Client Client { get; set; }
    }
}
