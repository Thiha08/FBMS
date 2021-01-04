using FBMS.Core.Attributes;

namespace FBMS.Core.Ctos
{
    [CrawlerEntity(XPath = "//table[starts-with(@id, 'SubAccsWinLose_')]//tr")]
    public class ActiveMemberCto : BaseCto
    {
        [CrawlerField(Expression = ".//a[starts-with(@id, 'SubAccsWinLose_')]/text()", SelectorType = SelectorType.XPath)]
        public string UserName { get; set; }
    }
}
