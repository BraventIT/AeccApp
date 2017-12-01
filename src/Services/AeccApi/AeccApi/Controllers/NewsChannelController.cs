using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using HtmlAgilityPack;
using AeccApi.Models;
using Aecc.Models;

namespace AeccApi.Controllers.API
{
    [Produces("application/json")]
    [Route("api/NewsChannel")]
    public class NewsChannelController : Controller
    {
        static List<NewsModel> _newsCache = new List<NewsModel>();
        static DateTime _lastUpdate;

        private const int MAXCACHEITEMS = 50;

        private NewsOptions newsOptions;
        private object _operationObj = new object();

        public NewsChannelController(IOptions<NewsOptions> options)
        {
            newsOptions = options.Value;
        }

        [HttpGet]
        public IActionResult GetNews(int? numNewsToLoad)
        {
            if (_lastUpdate < DateTime.UtcNow.AddHours(-newsOptions.TimeToLiveHrs))
            {
                HtmlWeb web = new HtmlWeb();

                var htmlDoc = web.Load(newsOptions.UrlNews);

                var nodes = htmlDoc.DocumentNode.SelectNodes("//div[@class=\"listadoItems\"]")
                    .Take(newsOptions.NumNewsToLoad)
                    .Reverse();

                Parallel.ForEach(
                    nodes,
                    new ParallelOptions() { MaxDegreeOfParallelism = newsOptions.NumNewsToLoad },
                    node => ProcessHtmlNode(node));

                _lastUpdate = DateTime.UtcNow;
            }

            var result = _newsCache
                    .Take(numNewsToLoad.HasValue ? numNewsToLoad.Value : newsOptions.NumNewsToLoad)
                    .ToList();

            return Ok(result);
        }

        private void ProcessHtmlNode(HtmlNode node)
        {
            var newData = ExtractNews(node);

            lock (_operationObj)
            {
                if (_newsCache.FirstOrDefault(n => n.NewsId == newData.NewsId) == null)
                {
                    _newsCache.Insert(0, newData);

                    if (_newsCache.Count > MAXCACHEITEMS)
                    {
                        _newsCache.RemoveAt(_newsCache.Count - 1);
                    }
                }
            }
        }

        private NewsModel ExtractNews(HtmlNode node)
        {
            NewsModel result = new NewsModel();

            var ahref = node.Descendants("a").FirstOrDefault();
            result.Title = ahref.InnerText;
            result.NewsId = newsOptions.UrlBase + ahref.Attributes["href"].Value;

            HtmlWeb web = new HtmlWeb();
            var htmlDoc = web.Load(result.NewsId);
            var nodeMainContent = htmlDoc.DocumentNode.SelectSingleNode("//div[@id=\"mainContent\"]");


            result.Imagen = newsOptions.UrlBase  + nodeMainContent.Descendants("img").FirstOrDefault()?.Attributes["src"].Value;
            result.Content = string.Join(Environment.NewLine,
                   nodeMainContent.Descendants("span")
                    .Where(x => x.InnerText != string.Empty)
                    .Select(x => x.InnerText));

            return result;
        }
    }
}