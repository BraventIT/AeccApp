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

            IocContainer.RegisterAsSingleton<IUserService, UserService>();
            IocContainer.RegisterAsSingleton<IHomeAddressesDataService, HomeAddressesDataService>();
            
            IocContainer.RegisterAsSingleton<IGoogleMapsPlaceService, GoogleMapsPlaceService>();
            IocContainer.Register<IHttpRequestProvider, HttpRequestProvider>();
        }

        public static INavigationService NavigationService
        {
            get { return Resolve<INavigationService>(); }
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
