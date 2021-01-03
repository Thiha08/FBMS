using System;
using System.Collections.Generic;
using System.Text;

namespace FBMS.Core.Dtos.Filters
{
    public class ClientFilterDto : BaseFilterDto
    {
        public string Account { get; set; }

        public bool? Status { get; set; }
    }
}
