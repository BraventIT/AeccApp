using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AeccApp.Core.Services
{
    public abstract class BaseDataListsService<T>: BaseReferredDataService<List<T>>
    {
        protected List<T> _data;

        public virtual int MaxItems { get { return -1; } }

        public int Count { get { return _data?.Count ?? 0; } }

        public async Task<List<T>> GetListAsync()
        {
            if (_data == null)
                _data = await LoadAsync() ?? new List<T>();

            return _data;
        }

        protected async Task InsertOrUpdateDataAsync(Func<T, bool> findPredicate, T data)
        {
            List<T> dataList = await GetListAsync();
            var oldData = dataList.FirstOrDefault(findPredicate);
            if (oldData != null)
            {
                dataList.Remove(oldData);
            }
            dataList.Insert(0, data);

            if (MaxItems>0 && dataList.Count > MaxItems)
            {
                dataList.RemoveAt(dataList.Count - 1);
            }
            Save(dataList);
        }

        protected async Task InsertOrIgnoreDataAsync(Func<T, bool> findPredicate, T data)
        {
            bool requiredSave = false;

            List<T> dataList = await GetListAsync();
            var oldData = dataList.FirstOrDefault(findPredicate);
            if (oldData == null)
            {
                dataList.Insert(0, data);
                requiredSave = true;

                if (MaxItems > 0 && dataList.Count > MaxItems)
                {
                    dataList.RemoveAt(dataList.Count - 1);
                }
            }

            if (requiredSave)
                Save(dataList);
        }
    }
}
