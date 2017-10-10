using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AeccApp.Core.Models.News
{
   public class NewsModel
    {

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


    }
}
