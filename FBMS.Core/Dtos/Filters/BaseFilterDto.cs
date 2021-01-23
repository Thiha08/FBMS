using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Text.Json.Serialization;

namespace FBMS.Core.Dtos.Filters
{
    public class BaseFilterDto
    {
        [Browsable(false)]
        [JsonIgnore]
        public bool LoadChildren { get; set; }

        [Browsable(false)]
        [JsonIgnore]
        public bool IsPagingEnabled { get; set; }

        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
