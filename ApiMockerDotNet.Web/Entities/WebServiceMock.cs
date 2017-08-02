using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiMockerDotNet.Web.Entities
{
    public class WebServiceMock
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string Verb { get; set; }
        public string MockFile { get; set; }
        public int HttpStatus { get; set; }
        public string ContentType { get; set; }
    }
}
