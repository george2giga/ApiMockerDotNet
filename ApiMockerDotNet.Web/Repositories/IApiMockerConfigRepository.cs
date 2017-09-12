using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiMockerDotNet.Web.Entities;

namespace ApiMockerDotNet.Web.Repositories
{
    public interface IApiMockerConfigRepository
    {
        ApiMockerConfig Get(string fileName);
    }
}
