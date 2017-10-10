using AeccApp.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AeccApp.Core.Services
{
    public interface INavigationService
    {
        ViewModelBase PreviousPageViewModel { get; }

        void Initialize();

        Task NavigateToAsync<TViewModel>(bool isModal = false) where TViewModel : ViewModelBase;

        Task NavigateToAsync<TViewModel>(object parameter, bool isModal = false) where TViewModel : ViewModelBase;

        Task NavigateBackAsync();

        Task RemoveBackStackAsync();

        Task RemoveLastFromBackStackAsync();
    }
}
