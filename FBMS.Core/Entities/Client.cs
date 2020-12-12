using FBMS.Core.Attributes;
using FBMS.SharedKernel;
using FBMS.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FBMS.Core.Entities
{
    [CrawlerEntity(XPath = "//table[starts-with(@id, 'SubAccsWinLose_')]//tr")]
    public class Client : BaseEntity, IAggregateRoot
    {
        [CrawlerField(Expression = ".//a[starts-with(@id, 'SubAccsWinLose_')]/text()", SelectorType = SelectorType.XPath)]
        public string Account { get; set; }

        public List<Transaction> Transactions { get; set; }

    }
}
