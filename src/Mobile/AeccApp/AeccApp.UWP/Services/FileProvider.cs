using AeccApp.Core.Services;
using AeccApp.UWP.Services;
using System;
using System.Threading.Tasks;
using Windows.Storage;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileProvider))]
namespace AeccApp.UWP.Services
{
    public class FileProvider : IFileProvider
    {
        private StorageFolder RootFolder { get; } = ApplicationData.Current.LocalFolder;

        public async Task DeleteFileAsync(string filename)
        {
            var file = await RootFolder.GetFileAsync(filename);
            await file.DeleteAsync();
        }

        public bool FileExists(string filename)
        {
            try
            {
                RootFolder.GetFileAsync(filename).AsTask().Wait();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<string> LoadTextAsync(string filename)
        {
            StorageFile sampleFile = await RootFolder.GetFileAsync(filename);
            string text = await FileIO.ReadTextAsync(sampleFile);
            return text;
        }

        public async Task SaveTextAsync(string filename, string text)
        {
            StorageFile sampleFile = await RootFolder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(sampleFile, text);
        }
    }
}
