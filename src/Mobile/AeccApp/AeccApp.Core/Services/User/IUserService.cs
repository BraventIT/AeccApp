using AeccApp.Core.Models;
using System;
using System.Threading.Tasks;

namespace AeccApp.Core.Services
{
    public interface IUserService
    {
        Task<UserData> GetUserAsync();
    }
}
