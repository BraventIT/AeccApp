using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;
using System;
using System.Diagnostics;

namespace AeccApp.Core.Services
{
    public abstract class BaseReferredDataService<T>
    {
        private Timer _saveTimer;

        protected readonly IFileProvider _fileProvider;

        public abstract string FileName { get; }

        public BaseReferredDataService()
        {
            _fileProvider = ServiceLocator.FileProvider;
        }

        protected async Task<T> LoadAsync()
        {
            return default(T);

            if (!_fileProvider.FileExists(FileName))
                return default(T);
            string jsonString = await _fileProvider.LoadTextAsync(FileName);
            return await Task.Run(() => JsonConvert.DeserializeObject<T>(jsonString));
        }

        protected void Save(T data)
        {
            _saveTimer?.Dispose();
            _saveTimer = new Timer(SaveTimerCallback, data, 2000, Timeout.Infinite);

        }

        private async void SaveTimerCallback(object state)
        {
            Debug.WriteLine($"Saving File {FileName}");
            string jsonString = await Task.Run(() => JsonConvert.SerializeObject(state));
            await _fileProvider.SaveTextAsync(FileName, jsonString);
            _saveTimer.Dispose();
        }
    }
}
