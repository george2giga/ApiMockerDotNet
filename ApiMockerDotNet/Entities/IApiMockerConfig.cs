using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiMockerDotNet.Entities
{
    public interface IApiMockerConfig
    {
       //string Note { get; set; }
       //string MocksFolder { get; set; }
       List<WebServiceMock> ServiceMocks { get; set; }
       WebServiceMock GetServiceMockByUrl(string url);
       WebServiceMock GetStartsWith(string url);
    }
}
