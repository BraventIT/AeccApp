using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AeccApp.Core.ViewModels
{
    public interface INavigableViewModel
    {
        Task InitializeAsync(object parameter);
        Task ActivateAsync();
        void Deactivate();

        bool ViewIsInitialized { get; set; }
        bool OnBackButtonPressed();
    }
}
