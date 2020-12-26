using FBMS.Core.Attributes;
using FBMS.SharedKernel;
using FBMS.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace FBMS.Core.Dtos.Auth
{
    [CrawlerEntity(XPath = "//form[@id='form1']")]
    public class SignInCrawlingDto : BaseEntity, IAggregateRoot
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
