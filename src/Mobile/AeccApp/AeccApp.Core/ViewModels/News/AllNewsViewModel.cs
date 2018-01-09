using Aecc.Models;
using AeccApp.Core.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using AeccApp.Core.Extensions;
using System.Linq;
using System.Collections.ObjectModel;
using System.Threading;

namespace AeccApp.Core.ViewModels
{
    public class AllNewsViewModel : ViewModelBase
    {
        private INewsDataService NewsDataService { get; } = ServiceLocator.NewsDataService;
        private INewsRequestService NewsService { get; } = ServiceLocator.NewsService;

        #region Contructor & Initialize
        public AllNewsViewModel()
        {
            NewsList = new ObservableCollection<NewsModel>();
        }

        public override async Task ActivateAsync()
        {
            await ExecuteOperationAsync(FillNewsAsync);
            await ExecuteOperationQuietlyAsync(cancelToken => TryToUpdateNewsAsync(cancelToken));
        }
        #endregion

        #region Propeties
        private ObservableCollection<NewsModel> newsList;
        public ObservableCollection<NewsModel> NewsList
        {
            get { return newsList; }
            set { Set(ref newsList, value); }
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
        }
        #endregion

        #region Private Methods
        private async Task FillNewsAsync()
        {
            var news = await NewsDataService.GetListAsync();
            if (news.Any())
            {
                NewsList.SyncExact(news);
            }
        }

        private async Task TryToUpdateNewsAsync(CancellationToken cancelToken)
        {
            var today = DateTime.Today.ToUniversalTime();
            int numItemsRequired = NewsDataService.MaxItems;
            if (Settings.LastNewsChecked != today || NewsDataService.Count < numItemsRequired)
            {
                var news = await NewsService.GetNewsAsync(cancelToken, numItemsRequired);

                foreach (var newData in news.Reverse())
                {
                    await NewsDataService.InsertOrUpdateAsync(newData);
                }

                await FillNewsAsync();

                Settings.LastNewsChecked = today;
            }
        }
        #endregion
    }
}
