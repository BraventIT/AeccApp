using Newtonsoft.Json;
using System.Threading.Tasks;

namespace AeccApp.Core.Services
{
    public abstract class BaseDataService<T>
    {
        protected readonly IFileProvider _fileProvider;

        public abstract string FileName { get; }

        public BaseDataService()
        {
            _fileProvider = ServiceLocator.FileProvider;
        }

        protected async Task<T> LoadAsync()
        {
            if (!_fileProvider.FileExists(FileName))
                return default(T);
            string jsonString = await _fileProvider.LoadTextAsync(FileName);
            return await Task.Run(() => JsonConvert.DeserializeObject<T>(jsonString));
        }

        protected async Task SaveAsync(T data)
        {
            string jsonString = await Task.Run(() => JsonConvert.SerializeObject(data));
            await _fileProvider.SaveTextAsync(FileName, jsonString);
        }
    }
}
