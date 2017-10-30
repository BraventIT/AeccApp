using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using AeccApp.Internationalization.Properties;
using AeccApp.Core.Services;
using System.Diagnostics;
using System.Threading;

namespace AeccApp.Core.ViewModels
{
    public abstract class ViewModelBase : BaseNotifyProperty, INavigableViewModel
    {
        protected INavigationService NavigationService { get; } = ServiceLocator.NavigationService;

        private static CancellationToken _currentToken;
        private static CancellationTokenSource _cancelTokenSource;

        /// <summary>
        /// ViewModelBase contains instances for GlobalSettings and LocalizationResources
        /// </summary>
        #region Properties
        protected GlobalSetting GSetting { get { return GlobalSetting.Instance; } }

        private bool _viewIsInitialized;
        public bool ViewIsInitialized
        {
            get { return _viewIsInitialized; }
            set { Set(ref _viewIsInitialized, value); }
        }

        private bool _isVolunteer;
        public bool IsVolunteer
        {
            get { return GSetting.User?.IsVolunteer ?? false; }
            set
            {
                if (Set(ref _isVolunteer, value))
                {
                    GSetting.User.IsVolunteer = _isVolunteer;
                }
            }
        }

        private int _busyOperations = 0;

        private bool _isBusy;

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                if (Set(ref _isBusy, value))
                { OnIsBusyChanged(); }
            }
        }

        private object _operationObj = new object();
        protected void StartOperation()
        {
            lock (_operationObj) 
                _busyOperations++;

            IsBusy = _busyOperations != 0;
        }

        protected void FinishOperation()
        {
            lock (_operationObj)
                _busyOperations--;

            IsBusy = _busyOperations != 0;
        }
        #endregion

        #region Localization Resources
        protected virtual ResourceManager LocalizationResourceManager { get; } = LocalizationResourcesAecc.ResourceManager;

        /// <summary>
		/// Obtiene el texto desde recursos para la key pasada (index).
		/// </summary>
		/// <param name="index">La "KEY" del texto de recursos que queramos obtener.</param>
		/// <returns>El texto traducido a la cultura establecida.</returns>
		public string this[string index]
        {
            get
            {
                try
                {
                    return LocalizationResourceManager.GetString(index) ?? $"_{index}";
                }
                catch (Exception)
                {
                    if (Debugger.IsAttached)
                    {
                        Debugger.Break();
                    }

                    return $"_{index}";
                }

            }
        }
        #endregion

        public virtual Task InitializeAsync(object navigationData)
        {
            return Task.CompletedTask;
        }

        public virtual Task ActivateAsync()
        {
            return Task.CompletedTask;
        }

        public virtual void Deactivate()  { }

        protected async Task ExecuteOperationAsync(Func<Task> executeAction, Action finallyAction = null)
        {
            try
            {
                StartOperation();

                await executeAction();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                finallyAction?.Invoke();

                FinishOperation();
            }
        }

        protected async Task ExecuteOperationAsync(Func<CancellationToken, Task> executeAction, Action finallyAction = null)
        {
            try
            {
                StartOperation();

                if (!_currentToken.IsCancellationRequested)
                    await executeAction(_currentToken);
            }
            catch (OperationCanceledException ex)
            {
                Debug.WriteLine(ex);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                finallyAction?.Invoke();

                FinishOperation();
            }
        }

        public static void UpdateToken()
        {
            if (_cancelTokenSource == null || _cancelTokenSource.IsCancellationRequested)
                _cancelTokenSource = new CancellationTokenSource();
            _currentToken = _cancelTokenSource.Token;
        }

        public static void TryCancelToken()
        {
            if (_cancelTokenSource != null && !_cancelTokenSource.IsCancellationRequested)
                _cancelTokenSource.Cancel();
        }

        protected virtual void OnIsBusyChanged() { }


        /// <summary>
        /// false is default value when system call back press
        /// </summary>
        /// <returns></returns>
        public virtual bool OnBackButtonPressed()
        {
            return false;
        }
     
       
    }
}
