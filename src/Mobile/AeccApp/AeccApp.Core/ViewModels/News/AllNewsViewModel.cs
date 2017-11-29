using Aecc.Models;
using AeccApp.Core.Models;
using AeccApp.Core.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using System.Linq;

namespace AeccApp.Core.ViewModels
{
    public class AllNewsViewModel : ViewModelBase
    {
        private INewsDataService NewsDataService { get; } = ServiceLocator.NewsDataService;
        private INewsRequestService NewsService { get; } = ServiceLocator.NewsService;

        public override async Task ActivateAsync()
        {
            await ExecuteOperationAsync(async cancelToken =>
            {
                var today = DateTime.Today.ToUniversalTime();
                int numItemsRequired = NewsDataService.MaxItems / 2;
                if (Settings.LastNewsChecked != today || NewsDataService.Count < numItemsRequired)
                {
                    var news = await NewsService.GetNewsAsync(cancelToken, numItemsRequired);

                    foreach (var newData in news.Reverse())
                    {
                        await NewsDataService.InsertOrUpdateAsync(newData);
                    }
                    Settings.LastNewsChecked = today;
                }

                NewsList = await NewsDataService.GetListAsync();
            });
        }


        #region Propeties
        private IEnumerable<NewsModel> newsList;
        public IEnumerable<NewsModel> NewsList
        {
            get
            {
                return newsList;
            }
            set
            {
                Set(ref newsList, value);

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
                    (chooseNewCommand = new Command(OnChooseNew, o => !IsBusy));
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
