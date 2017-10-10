using AeccApp.Core.Models.News;
using AeccApp.Internationalization.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AeccApp.Core.ViewModels
{
    public class AllNewsViewModel : ViewModelBase
    {
     

        #region Propeties
        private List<NewsModel> newsList;

        public List<NewsModel> NewsList
        {
            get
            {
                if (newsList == null)
                {
                    newsList = new List<NewsModel>()
                    {
                        new NewsModel("id1", null, "Titulo de noticia mock 1", "Esto representa el contenido resumido de una noticia lorem ipsum"),
                        new NewsModel("id2", null, "Titulo de noticia mock 2", "Esto representa el contenido resumido de una noticia lorem ipsum 2"),
                        new NewsModel("id3", null, "Titulo de noticia mock 3", "Esto representa el contenido resumido de una noticia lorem ipsum 3")
                    };
                }
                return newsList;
            }
        }
        #endregion

        #region Commands
        private Command chooseNewCommand;
        public ICommand ChooseNewCommand
        {
            get
            {
                return chooseNewCommand ??
                    (chooseNewCommand = new Command(OnChooseNew, (o) => !IsBusy));
            }
        }

        private async void OnChooseNew(object obj)
        {
            var selectedNew = obj as NewsModel;                                 
            await NavigationService.NavigateToAsync<NewsDetailViewModel>(selectedNew);
            await NavigationService.RemoveLastFromBackStackAsync();
        }
        #endregion
    }
}
