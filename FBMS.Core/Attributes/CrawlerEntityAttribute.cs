using System;
using System.Collections.Generic;
using System.Text;

namespace FBMS.Core.Attributes
{
    public class CrawlerEntityAttribute : Attribute
    {
        public string XPath { get; set; }
    }
}
