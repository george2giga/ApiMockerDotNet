using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiMockerDotNet.Entities;

namespace ApiMockerDotNet.Repositories
{
    public interface IApiMockerConfigRepository
    {
        Task<ApiMockerConfig> GetConfig(string fileName);
        Task<string> GetMockedResponse(string fileName, string folder = null);
    }
}
