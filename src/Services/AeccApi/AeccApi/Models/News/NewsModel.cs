using Newtonsoft.Json;

namespace Aecc.Models
{
    [JsonObject(IsReference = true)]

    public class NewsModel
    {

        public NewsModel() { }
        public NewsModel(string newsId ,string imagen, string title, string content)
        {
            //constructor creado para poder mockear
            this.newsId = newsId;
            this.imagen = imagen;
            this.title = title;
            this.content = content;
        }

        private string newsId;

        public string NewsId
        {
            get { return newsId; }
            set { newsId = value; }
        }


        private string imagen;

        public string Imagen
        {
            get { return imagen; }
            set { imagen = value; }
        }

        private string title;

        public string Title
        {
            get { return title; }
            set { title = value; }
        }


        private string content;

        public string Content
        {
            get { return content; }
            set { content = value; }
        }

#if !SERVICE
        public string ContentSummary
        {
            get { return content.Length > 100 ? content.Substring(0, 100) + "..." : content; }
        }
#endif

    }
}
