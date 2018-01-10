using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AeccApp.Core.ViewModels;
using Xamarin.Forms;
using System.Reflection;
using System.Globalization;
using AeccApp.Core.Views;
using AeccApp.Core.Helpers;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;

namespace AeccApp.Core.Services
{
    public class NavigationService : INavigationService
    {
        public ViewModelBase PreviousPageViewModel
        {
            get
            {
                var mainPage = Application.Current.MainPage as NavigationPage;
                var viewModel = mainPage.Navigation.NavigationStack[mainPage.Navigation.NavigationStack.Count - 2].BindingContext;
                return viewModel as ViewModelBase;
            }
        }

        public void Initialize()
        {
            var navigationPage = new NavigationPage(CreatePage(typeof(DashboardViewModel), null));
            //navigationPage.Popped += OnPagePopped;
            Application.Current.MainPage = navigationPage;
        }

        public async Task NavigateToAsync<TViewModel>(object parameter = null, bool isModal = false) where TViewModel : ViewModelBase
        {
            Page page = CreatePage(typeof(TViewModel), parameter);

            var navigationPage = Application.Current.MainPage as NavigationPage;
            if (page is DashboardView || navigationPage == null)
            {
                //navigationPage.Popped -= OnPagePopped;
                navigationPage = new NavigationPage(page); ;
                //navigationPage.Popped += OnPagePopped;
                Application.Current.MainPage = navigationPage;
            }
            else
            {
                if (isModal)
                    await navigationPage.Navigation.PushModalAsync(page);
                else
                    await navigationPage.PushAsync(page);
            }
           
            await (page.BindingContext as INavigableViewModel).InitializeAsync(parameter);
        }

        public async Task NavigateBackAsync()
        {
            var mainPage = Application.Current.MainPage as NavigationPage;
            if (mainPage != null)
            {
                var page = (mainPage.Navigation.ModalStack.Any()) ?
                     await mainPage.Navigation.PopModalAsync() :
                     await mainPage.Navigation.PopAsync();
            }
        }

        public Task RemoveBackStackAsync()
        {
            var mainPage = Application.Current.MainPage as NavigationPage;

            if (mainPage != null)
            {
                for (int i = 0; i < mainPage.Navigation.NavigationStack.Count - 1; i++)
                {
                    var page = mainPage.Navigation.NavigationStack[i];
                    mainPage.Navigation.RemovePage(page);
                }
            }

            return Task.CompletedTask;
        }

        public Task RemoveLastFromBackStackAsync()
        {
            var mainPage = Application.Current.MainPage as NavigationPage;

            if (mainPage != null)
            {
                mainPage.Navigation.RemovePage(
                    mainPage.Navigation.NavigationStack[mainPage.Navigation.NavigationStack.Count - 2]);
            }

            return Task.CompletedTask;
        }

        private async void OnPagePopped(object sender, NavigationEventArgs e)
        {
            var mainPage = Application.Current.MainPage as NavigationPage;
            if (mainPage != null)
            {
                var page = mainPage.CurrentPage;

                var tabbedPage = page as TabbedPage;
                if (tabbedPage != null)
                {
                    page = tabbedPage.CurrentPage;
                }

                await (page.BindingContext as INavigableViewModel).ActivateAsync();
            }
        }

        private Page CreatePage(Type viewModelType, object parameter)
        {
            Type pageType = ViewModelPath.GetPageTypeForViewModel(viewModelType);
            if (pageType == null)
            {
                throw new Exception($"Cannot locate page type for {viewModelType}");
            }

            Page page = Activator.CreateInstance(pageType) as Page;
            return page;
        }

        #region Popups
        public async Task ShowPopupAsync(ViewModelBase viewModel, object parameter = null)
        {
            PopupPage popupPage = CreatePopupPage(viewModel.GetType());
            if (parameter != null)
            {
                await viewModel.InitializeAsync(parameter);
            }
            popupPage.BindingContext = viewModel;
            await PopupNavigation.PushAsync(popupPage);
        }

        public Task HidePopupAsync()
        {
            return PopupNavigation.PopAllAsync();
        }

        private PopupPage CreatePopupPage(Type viewModelType)
        {
            Type popupPageType = ViewModelPath.GetPopupPageTypeForViewModel(viewModelType);
            if (popupPageType == null)
            {
                throw new Exception($"Cannot locate popup page type for {viewModelType}");
            }

            PopupPage page = Activator.CreateInstance(popupPageType) as PopupPage;
            return page;
        }
        #endregion

    }
}
