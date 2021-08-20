using FBMS.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace FBMS.Core.Ctos
{
    [CrawlerEntity(XPath = "//table[starts-with(@id, 'SubAccsWLTrans')]//tr")]
    public class TransactionCto : BaseCto
    {
        [CrawlerField(Expression = ".//td[1]/text()", SelectorType = SelectorType.XPath)]
        public string SerialNumber { get; set; }

        [CrawlerField(Expression = ".//td[2]/text()", SelectorType = SelectorType.XPath)]
        public string UserName { get; set; }

        [CrawlerField(Expression = ".//td[3]/span[1]/text()", SelectorType = SelectorType.XPath)]
        public string TransactionNumber { get; set; }

        [CrawlerField(Expression = ".//td[3]//br[1]/following-sibling::text()[1]", SelectorType = SelectorType.XPath)]
        public string TransactionDate { get; set; }

        [CrawlerField(Expression = ".//td[4]/span[1]/text()[1]", SelectorType = SelectorType.XPath)]
        public string League { get; set; }

        [CrawlerField(Expression = ".//td[4]//span[1]/br[1]/following-sibling::text()[1]", SelectorType = SelectorType.XPath)]
        public string HomeTeam { get; set; }

        [CrawlerField(Expression = ".//td[4]//span[2]/text()", SelectorType = SelectorType.XPath)]
        public string AwayTeam { get; set; }

        [CrawlerField(Expression = ".//td[4]//span[2]/following-sibling::text()[1]", SelectorType = SelectorType.XPath)]
        public string FirstHalf { get; set; }

        [CrawlerField(Expression = ".//td[5]//span[1]/text()", SelectorType = SelectorType.XPath)]
        public string TransactionType { get; set; }

        [CrawlerField(Expression = ".//td[5]//br[1]/following-sibling::text()[1]", SelectorType = SelectorType.XPath)]
        public string Pricing { get; set; }

        [CrawlerField(Expression = ".//td[7]/text()", SelectorType = SelectorType.XPath)]
        public string Amount { get; set; }
    }
}
