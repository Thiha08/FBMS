using FBMS.Core.Ctos;
using HtmlAgilityPack;
using System.Collections.Generic;

namespace FBMS.Spider.Processor
{
    public interface ICrawlerProcessor
    {
        IEnumerable<TEntity> Process<TEntity>(HtmlDocument document) where TEntity : BaseCto;
    }
}
