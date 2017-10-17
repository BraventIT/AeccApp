using AeccApp.Core.Services;
using Foundation;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(AeccApp.iOS.Services.FileProvider))]
namespace AeccApp.iOS.Services
{
    public class FileProvider : IFileProvider
    {
        public bool FileExists(string filename)
        {
            return File.Exists(CreatePathToFile(filename));
        }

        public async Task<string> LoadTextAsync(string filename)
        {
            string path = CreatePathToFile(filename);
            using (StreamReader sr = File.OpenText(path))
                return await sr.ReadToEndAsync();
        }

        public async Task SaveTextAsync(string filename, string text)
        {
            string path = CreatePathToFile(filename);
            using (StreamWriter sw = File.CreateText(path))
                await sw.WriteAsync(text);
        }

        public static string DocumentsPath
        {
            get
            {
                var documentsDirUrl = NSFileManager.DefaultManager.GetUrls(NSSearchPathDirectory.DocumentDirectory, NSSearchPathDomain.User).Last();
                return documentsDirUrl.Path;
            }
        }

        static string CreatePathToFile(string fileName)
        {
            return Path.Combine(DocumentsPath, fileName);
        }
    }
}