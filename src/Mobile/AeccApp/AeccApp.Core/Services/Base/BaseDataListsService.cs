using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace AeccApp.Core.Services
{
    public abstract class BaseDataListsService<T>
    {
        protected readonly IFileProvider _fileProvider;
        protected List<T> _data;
        public abstract string FileName { get; }
        public BaseDataListsService()
        {
            _fileProvider = ServiceLocator.FileProvider;
        }
        public async Task<List<T>> GetListAsync()
        {
            if (_data == null)
            {
                _data = await LoadAsync();
            }
            return _data;
        }
        protected async Task<List<T>> LoadAsync()
        {
            if (!_fileProvider.FileExists(FileName))
                return null;
            string jsonString = await _fileProvider.LoadTextAsync(FileName);
            return await Task.Run(() => JsonConvert.DeserializeObject<List<T>>(jsonString));
        }
        protected async Task SaveAsync(IEnumerable<T> data)
        {
            string jsonString = await Task.Run(() => JsonConvert.SerializeObject(data));
            await _fileProvider.SaveTextAsync(FileName, jsonString);
        }
        protected async Task AddOrUpdateDataAsync(Func<T, bool> findPredicate, T data)
        {
            List<T> dataList = await GetListAsync() ?? new List<T>();
            var oldData = dataList.FirstOrDefault(findPredicate);
            if (oldData != null)
            {
                dataList.Remove(oldData);
            }
            dataList.Add(data);
            await SaveAsync(dataList);
        }
    }
}
