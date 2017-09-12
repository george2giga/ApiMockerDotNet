using System.IO;
using ApiMockerDotNet.Web.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ApiMockerDotNet.Web.Repositories
{
    public class ApiMockerConfigRepository : IApiMockerConfigRepository
    {
        private static IApiMockerConfigRepository _current = new ApiMockerConfigRepository();
        
        public static 

        private const string ConfigsFolder = "app-configs";
        
        public ApiMockerConfig Get(string fileName)
        {
            var fullFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", ConfigsFolder, fileName);
            if (!File.Exists(fullFilePath))
            {
                throw new FileNotFoundException($"File not found {fullFilePath} /n ApiMocker cannot start without a valid config file");
            }

            var fileContent = File.ReadAllText(fullFilePath);
            var serializerSettings = new JsonSerializerSettings {ContractResolver = new CamelCasePropertyNamesContractResolver()};
            ApiMockerConfig apiMockerConfig;

            try
            {
                var deserializedConfig = JsonConvert.DeserializeObject<ApiMockerConfig>(fileContent, serializerSettings);
                apiMockerConfig = deserializedConfig;
            }
            catch
            {
                throw new JsonReaderException($"Invalid JSON in file {fullFilePath}");
            }

            return apiMockerConfig;
        }
    }
}
