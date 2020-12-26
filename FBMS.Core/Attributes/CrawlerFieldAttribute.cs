using System;
using System.Collections.Generic;
using System.Text;

namespace FBMS.Core.Attributes
{
    public class CrawlerFieldAttribute : Attribute
    {
        public string Expression { get; set; }

        public string HtmlAttribute { get; set; }

        public SelectorType SelectorType { get; set; }
    }
}
