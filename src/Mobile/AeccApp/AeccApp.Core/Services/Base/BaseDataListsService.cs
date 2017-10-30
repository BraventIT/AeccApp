using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace AeccApp.Core.Services
{
    public abstract class BaseDataListsService<T>: BaseDeferredDataService<List<T>>
    {
        protected List<T> _data;
       
        public async Task<List<T>> GetListAsync()
        {
            if (_data == null)
                _data = await LoadAsync() ?? new List<T>();

            return _data;
        }

        protected async Task AddOrUpdateDataAsync(Func<T, bool> findPredicate, T data)
        {
            List<T> dataList = await GetListAsync();
            var oldData = dataList.FirstOrDefault(findPredicate);
            if (oldData != null)
            {
                dataList.Remove(oldData);
            }
            dataList.Add(data);
            Save(dataList);
        }
    }
}
