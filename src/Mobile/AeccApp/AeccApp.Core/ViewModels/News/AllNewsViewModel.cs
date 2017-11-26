using Aecc.Models;
using AeccApp.Core.Models;
using AeccApp.Core.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AeccApp.Core.ViewModels
{
    public class AllNewsViewModel : ViewModelBase
    {
        private INewsRequestService NewsService { get; } = ServiceLocator.NewsService;

        public override async Task ActivateAsync()
        {
         

            await ExecuteOperationAsync(async cancelToken =>
            {
         
                NewsList = await NewsService.GetNewsAsync(cancelToken, 5);
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
