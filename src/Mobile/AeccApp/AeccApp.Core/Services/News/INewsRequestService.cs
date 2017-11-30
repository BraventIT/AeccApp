using Aecc.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AeccApp.Core.Services
{
    public interface INewsRequestService
    {
        Task<IList<NewsModel>> GetNewsAsync(CancellationToken cancelToken, int? numNewsToLoad = 2);

    }
}
