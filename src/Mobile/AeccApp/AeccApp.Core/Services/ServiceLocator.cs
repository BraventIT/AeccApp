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
            
            IocContainer.RegisterAsSingleton<IGoogleMapsPlaceService, GoogleMapsPlaceService>();
            IocContainer.Register<IRequestProvider, RequestProvider>();
        }

        public static T Resolve<T>()
        {
            return IocContainer.Resolve<T>();
        }
    }
}
