using AeccApp.Core.Helpers;
using Xamarin.Forms;

namespace AeccApp.Core.ViewModels
{
    public static class ViewModelLocator
    {
        #region Bindable Properties

        public static bool GetAutoWireViewModel(BindableObject obj)
        {
            return (bool)obj.GetValue(AutoWireViewModelProperty);
        }

        public static void SetAutoWireViewModel(BindableObject obj, bool value)
        {
            obj.SetValue(AutoWireViewModelProperty, value);
        }

        // Using a BindableProperty as the backing store for AutoWireViewModel.  This enables animation, styling, binding, etc...
        public static readonly BindableProperty AutoWireViewModelProperty =
            BindableProperty.CreateAttached("AutoWireViewModel", typeof(bool), typeof(ViewModelLocator), false, propertyChanged: OnAutoWireViewModelChanged);

        private static void OnAutoWireViewModelChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = bindable as Element;
            if (view == null)
            {
                return;
            }

            var viewType = view.GetType();
            var viewModelType = ViewModelPath.GetViewModelTypeForPage(viewType);
            if (viewModelType == null)
            {
                return;
            }
            var viewModel = IocContainer.Resolve(viewModelType);
            view.BindingContext = viewModel;
        }
        #endregion

        public static void RegisterDependencies()
        {
            //Add here further view models to navigate to
            // View models
            IocContainer.Register<VolunteerTestViewModel>();
            IocContainer.Register<LoginViewModel>();
            IocContainer.Register<DashboardViewModel>();
            IocContainer.Register<HomeViewModel>();
            IocContainer.Register<ChatViewModel>();
            IocContainer.Register<ProfileViewModel>();
            IocContainer.Register<RequestsViewModel>();
            IocContainer.Register<AllNewsViewModel>();
            IocContainer.Register<NewsDetailViewModel>();
            IocContainer.Register<NewRequestSelectAddressViewModel>();
            IocContainer.Register<AllYourRequestsListViewModel>();
            IocContainer.Register<NewHomeAddressViewModel>();
            IocContainer.Register<ChatEventViewModel>();
            IocContainer.Register<HomeAddressesListViewModel>();
            IocContainer.Register<NewHomeRequestChooseTypeViewModel>();
            IocContainer.Register<CompletingRequestViewModel>();
            IocContainer.Register<ChatRequestViewModel>();
        }
    }
}
