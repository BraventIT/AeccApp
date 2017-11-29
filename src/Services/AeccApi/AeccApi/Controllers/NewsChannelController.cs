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
        static List<NewsModel> _lastResult;
        static DateTime _lastUpdate;

        private NewsOptions newsOptions;
        public NewsChannelController(IOptions<NewsOptions> options)
        {
            newsOptions = options.Value;
        }

        [HttpGet]
        public IActionResult GetNews(int? numNewsToLoad)
        {
            List<NewsModel> result = new List<NewsModel>();

            if (_lastUpdate > DateTime.UtcNow.AddHours(-newsOptions.TimeToLiveHrs) && _lastUpdate != null)
            {
                result = _lastResult;
            }
            else
            {
                HtmlWeb web = new HtmlWeb();

                var htmlDoc = web.Load(newsOptions.UrlNews);


                var nodes = htmlDoc.DocumentNode.SelectNodes("//div[@class=\"listadoItems\"]").Take(
                    numNewsToLoad.HasValue ? numNewsToLoad.Value : newsOptions.NumNewsToLoad);


                Parallel.ForEach(
                    nodes,
                    new ParallelOptions() { MaxDegreeOfParallelism = 2 },
                    node => result.Add(ExtractNews(node)));

                _lastResult = result;
                _lastUpdate = DateTime.UtcNow;
            }
            return Ok(result);
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