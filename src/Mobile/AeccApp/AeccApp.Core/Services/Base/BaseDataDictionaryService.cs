using System.Collections.Generic;
using System.Threading.Tasks;

namespace AeccApp.Core.Services
{
    public abstract class BaseDataDictionaryService<T>: BaseDataService<Dictionary<string, T>>
    {
        protected Dictionary<string, T> _data;

        public async Task<T> GetAsync(string key)
        {
            if (_data == null)
                _data = await LoadAsync() ?? new Dictionary<string, T>();

            return (_data.ContainsKey(key)) ?
                _data[key] : default(T);
        }

        
        protected async Task AddOrUpdateDataAsync(string key, T value)
        {
            if (_data == null)
                _data = await LoadAsync() ?? new Dictionary<string, T>();

            _data[key] = value;
            await SaveAsync(_data);
        }
    }
}
