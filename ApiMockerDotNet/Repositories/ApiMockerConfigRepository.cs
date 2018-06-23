using System.Threading.Tasks;
using ApiMockerDotNet.Entities;
using ApiMockerDotNet.Utils;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ApiMockerDotNet.Repositories
{
    public class ApiMockerConfigRepository : IApiMockerConfigRepository
    {
        private readonly IFileSettingsProvider fileSettingsProvider;
        private readonly ILogger<ApiMockerConfigRepository> logger;
        private readonly string fileName;

        private const string ConfigsFolder = "app-configs";
        private const string MocksDefaultFolder = "app-mocks";

        public ApiMockerConfigRepository(IFileSettingsProvider fileSettingsProvider, ILogger<ApiMockerConfigRepository> logger, string fileName)
        {
            this.fileSettingsProvider = fileSettingsProvider;
            this.logger = logger;
            this.fileName = fileName;
        }



        public async Task<ApiMockerConfig> GetConfig()
        {
            var apiMockerConfig = new ApiMockerConfig();

            var fullFilePath = fileSettingsProvider.GetFullFilePath(fileName, ConfigsFolder);
            if (!fileSettingsProvider.FileExists(fullFilePath))
            {
                logger.LogError($"File not found {fullFilePath} /n ApiMockerDotNet cannot start without a valid config file");
                logger.LogError($"Please add a config file in the {ConfigsFolder} folder");
                //config not found, exit the method and try again             
                return apiMockerConfig;
            }

            var fileContent = await fileSettingsProvider.GetFileContent(fullFilePath);

            JsonSerializerSettings serializerSettings = new JsonSerializerSettings
            {
                MissingMemberHandling = MissingMemberHandling.Error,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            try
            {
                var deserializedConfig = JsonConvert.DeserializeObject<ApiMockerConfig>(fileContent, serializerSettings);
                apiMockerConfig = deserializedConfig;
                apiMockerConfig.IsLoaded = true;
            }
            catch
            {
                logger.LogError($"Invalid JSON in config file {fullFilePath}");
                logger.LogError($"Please add a valid file in the {ConfigsFolder} folder");
                //invalid json, killing the exception, try again with a valid file
            }

            return apiMockerConfig;
        }

        /// <summary>
        /// Returns the string response for the mocked service
        /// </summary>
        /// <param name="fileName">filename (including extensions) containing the service response</param>
        /// <param name="folder">full folder path, if empty default location (app relative) will be used </param>
        /// <returns></returns>
        public async Task<string> GetMockedResponse(string fileName, string folder = null)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                logger.LogWarning($"Filename is empty");
                return string.Empty;
            }

            var fullFilePath = string.IsNullOrEmpty(folder) ? fileSettingsProvider.GetFullFilePath(fileName, MocksDefaultFolder) : fileSettingsProvider.GetFullFilePath(fileName, folder, false);

            if (!fileSettingsProvider.FileExists(fullFilePath))
            {
                logger.LogInformation($"File not found {fullFilePath}");
                return string.Empty;
            }

            string content = await fileSettingsProvider.GetFileContent(fullFilePath);

            return content;
        }
    }
}
