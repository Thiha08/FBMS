using FBMS.Core.Attributes;

namespace FBMS.Core.Ctos
{
    [CrawlerEntity(XPath = "//table[starts-with(@id, 'MemberList_')]//tr")]
    public class MemberCto : BaseCto
    {
        [CrawlerField(Expression = ".//a[starts-with(@id, 'MemberList_')]/text()", SelectorType = SelectorType.XPath)]
        public string UserName { get; set; }
    }
}
