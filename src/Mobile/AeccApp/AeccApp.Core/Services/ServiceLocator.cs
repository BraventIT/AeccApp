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

            IocContainer.RegisterAsSingleton<IHospitalRequestService, HospitalRequestService>();
            IocContainer.RegisterAsSingleton<IMapPositionsDataService, MapPositionsDataService>();
            IocContainer.RegisterAsSingleton<IAddressesDataService, AddressesDataService>();
            IocContainer.RegisterAsSingleton<INewsDataService, NewsDataService>();
            IocContainer.RegisterAsSingleton<IEmailService, EmailService>();
            IocContainer.RegisterAsSingleton<IUserService, UserService>();
            IocContainer.RegisterAsSingleton<IHomeRequestService, HomeRequestService>();
            IocContainer.RegisterAsSingleton<INewsRequestService, NewsRequestService>();  

            IocContainer.RegisterAsSingleton<IHospitalRequestDataService, HospitalRequestDataService>();
            IocContainer.RegisterAsSingleton<IHomeRequestsDataService, HomeRequestsDataService>();
            IocContainer.RegisterAsSingleton<IHospitalRequestsTypesDataService, HospitalRequestsTypesDataService>();
            IocContainer.RegisterAsSingleton<IHomeRequestsTypesDataService, HomeRequestsTypesDataService>();
            


            IocContainer.RegisterAsSingleton<IGoogleMapsService, GoogleMapsService>();
            IocContainer.RegisterAsSingleton<IGoogleMapsService, GoogleMapsService>();
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

        public static IAddressesDataService AddressesDataService
        {
            get { return Resolve<IAddressesDataService>(); }
        }

        public static IHomeRequestsDataService HomeRequestsDataService
        {
            get { return Resolve<IHomeRequestsDataService>(); }
        }
        public static IHospitalRequestDataService HospitalRequestDataService
        {
            get { return Resolve<IHospitalRequestDataService>(); }
        }

        public static INewsDataService NewsDataService
        {
            get { return Resolve<INewsDataService>(); }
        }

        public static IHospitalRequestService HospitalRequestService
        {
            get { return Resolve<IHospitalRequestService>(); }
        }

        public static IMapPositionsDataService MapPositionsDataService
        {
            get { return Resolve<IMapPositionsDataService>(); }
        }

        public static IHospitalRequestsTypesDataService HospitalRequestsTypesDataService
        {
            get { return Resolve<IHospitalRequestsTypesDataService>(); }
        }
        public static IHomeRequestsTypesDataService HomeRequestsTypesDataService
        {
            get { return Resolve<IHomeRequestsTypesDataService>(); }
        }

        static IHomeRequestService _homeRequestService;
        public static IHomeRequestService HomeRequestService
        {
            set { _homeRequestService = value; }
            get
            {
                return (_homeRequestService != null) ?
                  _homeRequestService :
                  Resolve<IHomeRequestService>();
            }
        }

        static IEmailService _emailService;
        public static IEmailService EmailService
        {
            set { _emailService = value; }
            get
            {
                return (_emailService != null) ?
                  _emailService :
                  Resolve<IEmailService>();
            }
        }

        static INewsRequestService _newsService;
        public static INewsRequestService NewsService
        {
            set { _newsService = value; }
            get
            {
                return (_newsService != null) ?
                  _newsService :
                  Resolve<INewsRequestService>();
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

        public static IGoogleMapsService GoogleMapsService
        {
            get { return Resolve<IGoogleMapsService>(); }
        }

        public static IFileProvider FileProvider
        {
            get { return DependencyService.Get<IFileProvider>(); }
        }

        public static ILocationProviderSettings LocationProviderSettings
        {
            get { return DependencyService.Get<ILocationProviderSettings>(); }
        }
        public static INotificationService NotificationService
        {
            get { return DependencyService.Get<INotificationService>(); }
        }

        private static T Resolve<T>()
        {
            return IocContainer.Resolve<T>();
        }
    }
}
