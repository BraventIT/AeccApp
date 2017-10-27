using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AeccApp.Core.Services.Base
{
    public abstract class BaseDataDictionaryService <T>
    {
        protected readonly IFileProvider _fileProvider;
        protected Dictionary<string,T> _data;
        public abstract string FileName { get; }
        public BaseDataDictionaryService()
        {
            _fileProvider = ServiceLocator.FileProvider;
        }
        public async Task<T> GetAsync(string key)
        {
            if (_data == null)
            {
                _data = await LoadAsync();
            }

            return _data[key];
        }
        protected async Task<Dictionary<string, T>> LoadAsync()
        {
            if (!_fileProvider.FileExists(FileName))
                return null;
            string jsonString = await _fileProvider.LoadTextAsync(FileName);
            return await Task.Run(() => JsonConvert.DeserializeObject<Dictionary<string, T>>(jsonString));
        }

        protected async Task SaveAsync(Dictionary<string, T> data)
        {
            string jsonString = await Task.Run(() => JsonConvert.SerializeObject(data));
            await _fileProvider.SaveTextAsync(FileName, jsonString);
        }
        protected async Task AddOrUpdateDataAsync(string key, T value)
        {
            if (_data == null)
            {
                _data = await LoadAsync();
            }
            if (_data == null)
            {
                _data = new Dictionary<string, T>();
            }

            _data[key] = value;
            await SaveAsync(_data);
        }
    }
}
