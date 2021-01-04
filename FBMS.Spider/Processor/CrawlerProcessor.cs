using FBMS.Core.Attributes;
using FBMS.Core.Ctos;
using FBMS.Core.Extensions;
using HtmlAgilityPack;
using HtmlAgilityPack.CssSelectors.NetCore;
using System;
using System.Collections.Generic;

namespace FBMS.Spider.Processor
{
    public class CrawlerProcessor : ICrawlerProcessor
    {
        public IEnumerable<TEntity> Process<TEntity>(HtmlDocument document) where TEntity : BaseCto
        {
            var processorEntities = new List<TEntity>();

            var entityExpression = ReflectionHelper.GetEntityExpression<TEntity>();

            var entityNodes = document.DocumentNode.SelectNodes(entityExpression);

            foreach (HtmlNode entityNode in entityNodes)
            {
                var nameValueDictionary = GetColumnNameValuePairsFromHtml<TEntity>(entityNode);

                var processorEntity = ReflectionHelper.CreateNewEntity<TEntity>();
                foreach (var pair in nameValueDictionary)
                {
                    ReflectionHelper.TrySetProperty(processorEntity, pair.Key, pair.Value);
                }

                processorEntities.Add(processorEntity as TEntity);
            }

            return processorEntities;
        }

        private static Dictionary<string, object> GetColumnNameValuePairsFromHtml<TEntity>(HtmlNode entityNode) where TEntity : BaseCto
        {
            var columnNameValueDictionary = new Dictionary<string, object>();

            var propertyExpressions = ReflectionHelper.GetPropertyAttributes<TEntity>();

            foreach (var expression in propertyExpressions)
            {
                var columnName = expression.Key;
                object columnValue = null;
                var fieldExpression = expression.Value.Item2;
                string attribute = expression.Value.Item3;

                switch (expression.Value.Item1)
                {
                    case SelectorType.XPath:
                        var node = entityNode.SelectSingleNode(fieldExpression);
                        if (node != null)
                        {
                            columnValue = string.IsNullOrEmpty(attribute) ?
                                node.InnerText?.Replace("&nbsp;", "")?.Trim() :
                                node.Attributes[attribute].Value?.Replace("&nbsp;", "")?.Trim();
                        }
                        break;
                    case SelectorType.CssSelector:
                        var nodeCss = entityNode.QuerySelector(fieldExpression);
                        if (nodeCss != null)
                        {
                            columnValue = nodeCss.InnerText;
                        }
                        break;
                    case SelectorType.FixedValue:
                        if (Int32.TryParse(fieldExpression, out var result))
                        {
                            columnValue = result;
                        }
                        break;
                    default:
                        break;
                }
                columnNameValueDictionary.Add(columnName, columnValue);
            }

            return columnNameValueDictionary;
        }
    }
}
