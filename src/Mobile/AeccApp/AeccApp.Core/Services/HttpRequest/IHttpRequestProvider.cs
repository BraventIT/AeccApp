using System.Threading;
using System.Threading.Tasks;

namespace AeccApp.Core.Services
{
    public interface IHttpRequestProvider
    {
        Task<TResult> GetAsync<TResult>(string uri, CancellationToken cancelToken = default(CancellationToken), string token = "");

        Task<TResult> PostAsync<TResult>(string uri, TResult data, CancellationToken cancelToken = default(CancellationToken), string token = "", string header = "");

        Task DeleteAsync(string uri, CancellationToken cancelToken = default(CancellationToken), string token = "");
    }
}
