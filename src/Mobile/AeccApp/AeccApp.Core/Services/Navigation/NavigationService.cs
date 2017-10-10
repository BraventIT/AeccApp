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
            var navigationPage = new NavigationPage(CreatePage(typeof(LoginViewModel), null));
            navigationPage.Popped += OnPagePopped;
            Application.Current.MainPage = navigationPage;
        }

        public Task NavigateToAsync<TViewModel>(bool isModal = false) where TViewModel : ViewModelBase
        {
            return InternalNavigateToAsync(typeof(TViewModel), null, isModal);
        }

        public Task NavigateToAsync<TViewModel>(object parameter, bool isModal = false) where TViewModel : ViewModelBase
        {
            return InternalNavigateToAsync(typeof(TViewModel), parameter, isModal);
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

        private async Task InternalNavigateToAsync(Type viewModelType, object parameter, bool isModal)
        {
            Page page = CreatePage(viewModelType, parameter);

            var navigationPage = Application.Current.MainPage as NavigationPage;
            if (page is DashboardView || navigationPage == null)
            {
                navigationPage.Popped -= OnPagePopped;
                navigationPage = new NavigationPage(page); ;
                navigationPage.Popped += OnPagePopped;
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

           //     await (page.BindingContext as INavigableViewModel).StartAsync(null);
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
    }
}
