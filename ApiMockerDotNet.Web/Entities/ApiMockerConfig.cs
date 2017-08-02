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
        public List<WebServiceMock> WebServiceMocks { get; set; }

        public ApiMockerConfig()
        {
            this.WebServiceMocks = new List<WebServiceMock>();
        }

        public bool WebServiceMocksExists(string url)
        {
            return this.WebServiceMocks.Any(x => string.Equals(x.Url, url, StringComparison.OrdinalIgnoreCase));
        }

        public WebServiceMock GetWebServiceMock(string url)
        {
            return this.WebServiceMocks.FirstOrDefault(x => string.Equals(x.Url, url, StringComparison.OrdinalIgnoreCase));
        }
    }    
}
