using System;
using System.Collections.Generic;
using System.Text;

namespace FBMS.SharedKernel.Interfaces
{
    // Apply this marker interface only to aggregate root entities
    // Repositories will only work with aggregate roots, not their children
    public interface IAggregateRoot { }
}
