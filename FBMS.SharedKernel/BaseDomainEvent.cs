using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace FBMS.SharedKernel
{
    public abstract class BaseDomainEvent : INotification
    {
        public DateTime DateOccurred { get; protected set; } = DateTime.UtcNow;
    }
}
