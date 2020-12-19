using FBMS.Core.Attributes;
using FBMS.SharedKernel;
using FBMS.SharedKernel.Interfaces;
using System;

namespace FBMS.Core.Entities
{
    [CrawlerEntity(XPath = "//table[starts-with(@id, 'SubAccsWLTrans')]//tr")]
    public class Transaction : BaseEntity, IAggregateRoot
    {
        [CrawlerField(Expression = ".//td[1]/text()", SelectorType = SelectorType.XPath)]
        public string SerialNumber { get; set; }

        [CrawlerField(Expression = ".//td[2]/text()", SelectorType = SelectorType.XPath)]
        public string Account { get; set; }

        [CrawlerField(Expression = ".//td[3]/span[1]/text()", SelectorType = SelectorType.XPath)]
        public string TransactionNumber { get; set; }

        [CrawlerField(Expression = ".//td[3]//br[1]/following-sibling::text()[1]", SelectorType = SelectorType.XPath)]
        public string Date { get; set; }

        [CrawlerField(Expression = ".//td[4]/span[1]/text()[1]", SelectorType = SelectorType.XPath)]
        public string League { get; set; }

        [CrawlerField(Expression = ".//td[4]//span[1]/br[1]/following-sibling::text()[1]", SelectorType = SelectorType.XPath)]
        public string HomeTeam { get; set; }

        [CrawlerField(Expression = ".//td[4]//span[2]/text()", SelectorType = SelectorType.XPath)]
        public string AwayTeam { get; set; }

        [CrawlerField(Expression = ".//td[5]//span[1]/text()", SelectorType = SelectorType.XPath)]
        public string PricingType { get; set; }

        [CrawlerField(Expression = ".//td[5]//br[1]/following-sibling::text()[1]", SelectorType = SelectorType.XPath)]
        public string Pricing { get; set; }

        [CrawlerField(Expression = ".//td[7]/text()", SelectorType = SelectorType.XPath)]
        public string Amount { get; set; }

        public int ClientId { get; set; }

        public Client Client { get; set; }
    }
}
