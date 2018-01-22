using Newtonsoft.Json;

namespace ApiMockerDotNet.Entities
{
    public class WebServiceMock
    {
        public string Name { get; set; }
        [JsonProperty(Required = Required.Always)]
        public string Url { get; set; }
        [JsonProperty(Required = Required.Always)]
        public string Verb { get; set; }
        [JsonProperty(Required = Required.Always)]
        public string MockFile { get; set; }
        [JsonProperty(Required = Required.Always)]
        public int HttpStatus { get; set; }
        [JsonProperty(Required = Required.Always)]
        public string ContentType { get; set; }


    }
}
