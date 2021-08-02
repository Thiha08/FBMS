using FBMS.SharedKernel;
using FBMS.SharedKernel.Interfaces;

namespace FBMS.Core.Entities
{
    public class Setting : BaseEntity, IAggregateRoot
    {
        public string Name { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }

    }
}
