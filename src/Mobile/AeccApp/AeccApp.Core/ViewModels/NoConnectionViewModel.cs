using AeccApp.Internationalization.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AeccApp.Core.ViewModels
{
    public class NoConnectionViewModel : ViewModelBase
    {


        #region Commands
        private Command _refreshConnection;
        public ICommand RefreshConnection
        {
            get
            {
                return _refreshConnection ??
                    (_refreshConnection = new Command(OnRefreshConnection, (o) => !IsBusy));
            }
        }
        /// <summary>
        /// Refresh connection on screen tap
        /// </summary>
        /// <param name="obj"></param>
        private void OnRefreshConnection(object obj)
        {

            //TODO Check if there is connection, if not, pop NoConnectionView, and manage connection refresh here

        }



        #endregion

    }
}
