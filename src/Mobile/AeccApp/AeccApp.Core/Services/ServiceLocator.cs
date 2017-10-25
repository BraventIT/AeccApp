using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Xamarin.Forms;

namespace AeccApp.Core.Services
{
    public static class ServiceLocator
    {
        public static void RegisterDependencies()
        {
            // Services
            IocContainer.RegisterAsSingleton<INavigationService, NavigationService>();
            IocContainer.RegisterAsSingleton<IChatService, ChatService>();
            IocContainer.RegisterAsSingleton<IIdentityService, IdentityService>();

            IocContainer.RegisterAsSingleton<IHomeAddressesDataService, HomeAddressesDataService>();
            IocContainer.RegisterAsSingleton<IUserService, UserService>();
            IocContainer.RegisterAsSingleton<IHomeRequestService, HomeRequestService>();
            IocContainer.RegisterAsSingleton<IGoogleMapsPlaceService, GoogleMapsPlaceService>();
            IocContainer.RegisterAsSingleton<IGoogleMapsPlaceService, GoogleMapsPlaceService>();
            IocContainer.Register<IHttpRequestProvider, HttpRequestProvider>();

        }

        static INavigationService _navigationService;
        public static INavigationService NavigationService
        {
            set { _navigationService = value; }
            get
            {
                return (_navigationService != null) ?
                  _navigationService :
                  Resolve<INavigationService>();
            }
        }

        public static IChatService ChatService
        {
            get { return Resolve<IChatService>(); }
        }

        public static IIdentityService IdentityService
        {
            get { return Resolve<IIdentityService>(); }
        }

        public static IHomeAddressesDataService HomeAddressesDataService
        {
            get { return Resolve<IHomeAddressesDataService>(); }
        }

        static IHomeAddressesDataService _homeRequestService;
        public static IHomeAddressesDataService HomeRequestService
        {
            set { _homeRequestService = value; }
            get
            {
                return (_homeRequestService != null) ?
                  _homeRequestService :
                  Resolve<IHomeAddressesDataService>();
            }
        }
        public static IGeolocator GeolocatorService
        {
            get { return CrossGeolocator.Current; }
        }

        public static IPermissions PermissionsService
        {
            get { return CrossPermissions.Current; }
        }

        public static IGoogleMapsPlaceService GoogleMapsPlaceService
        {
            get { return Resolve<IGoogleMapsPlaceService>(); }
        }

        public static IFileProvider FileProvider
        {
            get { return DependencyService.Get<IFileProvider>(); }
        }

        private static T Resolve<T>()
        {
            return IocContainer.Resolve<T>();
        }
    }
}
