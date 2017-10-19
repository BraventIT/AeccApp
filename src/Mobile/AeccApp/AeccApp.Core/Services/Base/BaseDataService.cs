using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AeccApp.Core.Services
{
    public abstract class BaseDataService<T>
    {
        protected readonly IFileProvider _fileProvider;

        public BaseDataService()
        {
            _fileProvider = ServiceLocator.FileProvider;
        }

        protected async Task<List<T>> LoadAsync(string filename)
        {
            if (!_fileProvider.FileExists(filename))
                return null;

            string jsonString = await _fileProvider.LoadTextAsync(filename);
            return await Task.Run(() => JsonConvert.DeserializeObject<List<T>>(jsonString));
        }

        protected async Task SaveAsync(string filename, IEnumerable<T> data)
        {
            string jsonString = await Task.Run(() => JsonConvert.SerializeObject(data));
            await _fileProvider.SaveTextAsync(filename, jsonString);
        }

        protected async Task AddOrUpdateDataAsync(string filename, Func<T, bool> findPredicate, T data)
        {
            List<T> dataList = await LoadAsync(filename) ?? new List<T>();

            var oldData = dataList.FirstOrDefault(findPredicate);
            if (oldData != null)
            {
                dataList.Remove(oldData);
            }
            dataList.Add(data);
            await SaveAsync(filename, dataList);
        }
    }
}
