using AeccApp.Core.Services;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AeccApp.Core.ViewModels
{
    public class ProfileViewModel : ViewModelBase
    {
        private IIdentityService IdentityService { get; } = ServiceLocator.IdentityService;
  
        #region Properties
        public string Name
        {
            get { return GSetting.User?.Name; }
        }

        public string Description
        {
            get { return GSetting.User.Description; }
        }

        public string Email
        {
            get { return GSetting.User?.Email; }
        }

        public int? Age
        {
            get { return GSetting.User.Age; }
        }
        public string Gender
        {
            get { return GSetting.User.DisplayGender; }
        }
        
        #endregion

        #region Commands
        private Command _editProfileCommand;
        public ICommand EditProfileCommand
        {
            get
            {
                return _editProfileCommand ??
                    (_editProfileCommand = new Command(o => OnEditProfileAsync()));
            }
        }

        /// <summary>
        /// Opens webview to edit profile
        /// </summary>
        /// <returns></returns>
        private async Task OnEditProfileAsync()
        {
            await IdentityService.EditProfileAsync();
            NotifyPropertyChanged(nameof(Name));
            NotifyPropertyChanged(nameof(Email));
            NotifyPropertyChanged(nameof(Age));
            NotifyPropertyChanged(nameof(Gender));
        }

        #endregion

      
    }
}
