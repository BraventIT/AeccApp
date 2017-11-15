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
    [Route("api/News")]
    public class NewsChannelController : Controller
    {

        private NewsOptions newsOptions;
        public NewsChannelController(IOptions<NewsOptions> options)
        {
            newsOptions = options.Value;
        }
        [HttpGet]
        public IActionResult GetNews(int? numNewsToLoad)
        {
            List<NewsModel> result = new List<NewsModel>();

            HtmlWeb web = new HtmlWeb();

            var htmlDoc = web.Load(newsOptions.UrlNews);


            var nodes = htmlDoc.DocumentNode.SelectNodes("//div[@class=\"listadoItems\"]").Take(
                numNewsToLoad.HasValue ? numNewsToLoad.Value : newsOptions.NumNewsToLoad);


            foreach (var node in nodes)
            {
                result.Add(ExtractNews(node));
            }


            return Ok(result);
        }

        private NewsModel ExtractNews(HtmlNode node)
        {
            NewsModel result = new NewsModel();

            var ahref = node.Descendants("a").FirstOrDefault();
            result.Title = ahref.InnerText;
            result.NewsId = ahref.Attributes["href"].Value;

            HtmlWeb web = new HtmlWeb();
            var htmlDoc = web.Load(newsOptions.UrlBase + result.NewsId);
            var nodeMainContent = htmlDoc.DocumentNode.SelectSingleNode("//div[@id=\"mainContent\"]");


            result.Imagen = nodeMainContent.Descendants("img").FirstOrDefault()?.Attributes["src"].Value;
            result.Content = string.Join(Environment.NewLine,
                   nodeMainContent.Descendants("span")
                    .Where(x => x.InnerText != string.Empty)
                    .Select(x => x.InnerText));

            return result;
        }
    }
}