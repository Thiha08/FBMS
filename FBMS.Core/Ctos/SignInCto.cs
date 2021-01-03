using FBMS.Core.Attributes;
using FBMS.Core.Ctos;

namespace FBMS.Core.Ctos
{
    [CrawlerEntity(XPath = "//form[@id='form1']")]
    public class SignInCto : BaseCto
    {
        [CrawlerField(Expression = "//form[@id='form1']", HtmlAttribute = "action", SelectorType = SelectorType.XPath)]
        public string AuthUrl { get; set; }

        [CrawlerField(Expression = ".//input[@id='__EVENTVALIDATION']", HtmlAttribute = "value", SelectorType = SelectorType.XPath)]
        public string EventValidation { get; set; }

        [CrawlerField(Expression = ".//input[@id='__VIEWSTATE']", HtmlAttribute = "value", SelectorType = SelectorType.XPath)]
        public string ViewState { get; set; }

        [CrawlerField(Expression = ".//input[@id='__VIEWSTATEGENERATOR']", HtmlAttribute = "value", SelectorType = SelectorType.XPath)]
        public string ViewStateGenerator { get; set; }
    }
}
