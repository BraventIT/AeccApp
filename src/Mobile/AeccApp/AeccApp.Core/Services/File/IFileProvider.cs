using System.Threading.Tasks;

namespace AeccApp.Core.Services
{
    public interface IFileProvider
    {
        Task SaveTextAsync(string filename, string text);
        Task<string> LoadTextAsync(string filename);
        bool FileExists(string filename);
    }
}
