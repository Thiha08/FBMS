using FBMS.SharedKernel;
using FBMS.SharedKernel.Interfaces;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FBMS.Spider.Processor
{
    public interface ICrawlerProcessor
    {
        IEnumerable<T> Process<T>(HtmlDocument document) where T : BaseEntity, IAggregateRoot;
    }
}
