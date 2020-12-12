using FBMS.Core.Attributes;
using FBMS.SharedKernel;
using FBMS.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FBMS.Core.Entities
{
    [CrawlerEntity(XPath = "//*[@id='LeftSummaryPanel']/div[1]")]
    public partial class Catalog : BaseEntity, IAggregateRoot
    {
        [CrawlerField(Expression = "1", SelectorType = SelectorType.FixedValue)]
        public int CatalogBrandId { get; set; }

        [CrawlerField(Expression = "1", SelectorType = SelectorType.FixedValue)]
        public int CatalogTypeId { get; set; }

        public string Description { get; set; }

        [CrawlerField(Expression = "//*[@id='itemTitle']/text()", SelectorType = SelectorType.XPath)]
        public string Name { get; set; }

        public string PictureUri { get; set; }

        public decimal Price { get; set; }
    }
}
