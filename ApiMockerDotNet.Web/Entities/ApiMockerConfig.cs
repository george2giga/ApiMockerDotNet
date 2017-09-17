using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiMockerDotNet.Web.Entities
{
    public class ApiMockerConfig
    {    
        public string Note { get; set; }
        public string MocksFolder { get; set; }
        public bool LogRequestHeaders { get; set; }
        public List<WebServiceMock> ServiceMocks { get; set; }

        public ApiMockerConfig()
        {
            this.ServiceMocks = new List<WebServiceMock>();
        }

        public WebServiceMock GetServiceMock(string url)
        {
            return this.ServiceMocks.FirstOrDefault(x => string.Equals(x.Url, url, StringComparison.OrdinalIgnoreCase));
        }

        public bool IsDefaultMocksFolder => string.IsNullOrEmpty(this.MocksFolder);
    }    
}
