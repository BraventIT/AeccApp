using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AeccApp.Core.Helpers
{
    internal class ViewModelPath
    {
        internal static Type GetPageTypeForViewModel(Type viewModelType)
        {
            var viewName = viewModelType.FullName.Replace("Model", string.Empty);
            var viewModelAssemblyName = viewModelType.GetTypeInfo().Assembly.FullName;
            var viewAssemblyName = $"{viewName}, {viewModelAssemblyName}";
            var viewType = Type.GetType(viewAssemblyName);
            return viewType;
        }

        internal static Type GetViewModelTypeForPage(Type viewType)
        {
            var viewName = viewType.FullName.Replace(".Views.", ".ViewModels.");
            var viewAssemblyName = viewType.GetTypeInfo().Assembly.FullName;
            var viewModelName = $"{viewName}Model, {viewAssemblyName}";
            var viewModelType = Type.GetType(viewModelName);
            return viewModelType;
        }
    }
}
