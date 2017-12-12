using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using HtmlAgilityPack;
using AeccApi.Models;
using Aecc.Models;
using Microsoft.Extensions.Caching.Memory;

namespace AeccApi.Controllers.API
{
    [Produces("application/json")]
    [Route("api/NewsChannel")]
    public class NewsChannelController : Controller
    {
        private const int MAXCACHEITEMS = 50;
        private const string NEWSKEY = "News";
        private const string LASTUPDATEKEY = "News.LastUpdate";

        private IMemoryCache _cache;
        private NewsOptions _newsOptions;
    
        public NewsChannelController(IOptions<NewsOptions> options, IMemoryCache  memoryCache)
        {
            _newsOptions = options.Value;
            _cache = memoryCache;
        }

        [HttpGet]
        public IActionResult GetNews(int? numNewsToLoad)
        {
            DateTime lastUpdate = _cache.GetOrCreate(LASTUPDATEKEY, entry =>
            {
                entry.SetSlidingExpiration(CacheExpiration);
                return DateTime.MinValue;
            });
            List<NewsModel> newsCache = _cache.GetOrCreate(NEWSKEY, entry =>
            {
                entry.SetSlidingExpiration(CacheExpiration);
                return new List<NewsModel>();
            });

            if (lastUpdate < DateTime.UtcNow.AddHours(-_newsOptions.TimeToLiveHrs))
            {
                HtmlWeb web = new HtmlWeb();

                var htmlDoc = web.Load(_newsOptions.UrlNews);

                var nodes = htmlDoc.DocumentNode.SelectNodes("//div[@class=\"listadoItems\"]")
                    .Take(_newsOptions.NumNewsToLoad)
                    .Reverse();

                Parallel.ForEach(
                    nodes,
                    new ParallelOptions() { MaxDegreeOfParallelism = _newsOptions.NumNewsToLoad },
                    node => ProcessHtmlNode(node, newsCache));

                lastUpdate = DateTime.UtcNow;
                _cache.Set(NEWSKEY,newsCache, new MemoryCacheEntryOptions().SetSlidingExpiration(CacheExpiration));
                _cache.Set(LASTUPDATEKEY, lastUpdate, new MemoryCacheEntryOptions().SetSlidingExpiration(CacheExpiration));
            }

            var result = newsCache
                    .Take(numNewsToLoad.HasValue ? numNewsToLoad.Value : _newsOptions.NumNewsToLoad)
                    .ToList();

            return Ok(result);
        }

        private void ProcessHtmlNode(HtmlNode node, IList<NewsModel> newsCache)
        {
            var newData = ExtractNews(node);
            
            if (newsCache.FirstOrDefault(n => n.NewsId == newData.NewsId) == null)
            {
                newsCache.Insert(0, newData);

                if (newsCache.Count > MAXCACHEITEMS)
                {
                    newsCache.RemoveAt(newsCache.Count - 1);
                }
            }
        }

        private NewsModel ExtractNews(HtmlNode node)
        {
            NewsModel result = new NewsModel();

            var ahref = node.Descendants("a").FirstOrDefault();
            result.Title = ahref.InnerText;
            result.NewsId = _newsOptions.UrlBase + ahref.Attributes["href"].Value;

            HtmlWeb web = new HtmlWeb();
            var htmlDoc = web.Load(result.NewsId);
            var nodeMainContent = htmlDoc.DocumentNode.SelectSingleNode("//div[@id=\"mainContent\"]");


            result.Imagen = _newsOptions.UrlBase  + nodeMainContent.Descendants("img").FirstOrDefault()?.Attributes["src"].Value;
            result.Content = string.Join(Environment.NewLine,
                   nodeMainContent.Descendants("span")
                    .Where(x => x.InnerText != string.Empty)
                    .Select(x => x.InnerText));

            return result;
        }

        private TimeSpan CacheExpiration
        {
            get { return TimeSpan.FromDays(15); }
        }
    }
}