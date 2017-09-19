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
        private readonly IFileSettingsProvider _fileSettingsProvider;
        private readonly ILogger<ApiMockerConfigRepository> _logger;
        private static ApiMockerConfig _config;

        private const string ConfigsFolder = "app-configs";
        private const string MocksDefaultFolder = "app-mocks";

        public ApiMockerConfigRepository(IFileSettingsProvider fileSettingsProvider, ILogger<ApiMockerConfigRepository> logger)
        {
            _fileSettingsProvider = fileSettingsProvider;
            _logger = logger;
        }

        public async Task<ApiMockerConfig> GetConfig(string fileName)
        {
            if (_config == null)
            {
                var fullFilePath = _fileSettingsProvider.GetFullFilePath(fileName, ConfigsFolder);
                if (!_fileSettingsProvider.FileExists(fullFilePath))
                {
                    _logger.LogError($"File not found {fullFilePath} /n ApiMockerDotNet cannot start without a valid config file");
                    _logger.LogError($"Please add a config file in the {ConfigsFolder} folder");
                }

                var fileContent = await _fileSettingsProvider.GetFileContent(fullFilePath);
                var serializerSettings = new JsonSerializerSettings {ContractResolver = new CamelCasePropertyNamesContractResolver()};

                try
                {
                    var deserializedConfig = JsonConvert.DeserializeObject<ApiMockerConfig>(fileContent, serializerSettings);
                    _config = deserializedConfig;
                }
                catch
                {
                    _logger.LogError($"Invalid JSON in config file {fullFilePath}");
                    _logger.LogError($"Please add a valid file in the {ConfigsFolder} folder");
                }
            }

            return _config;
        }

        public async Task<string> GetMockedResponse(string fileName, string folder = null)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                _logger.LogWarning($"Filename is empty");
                return string.Empty;
            }

            var fullFilePath = string.IsNullOrEmpty(folder) ? _fileSettingsProvider.GetFullFilePath(fileName, MocksDefaultFolder) : _fileSettingsProvider.GetFullFilePath(fileName, folder, false);
            if (!_fileSettingsProvider.FileExists(fullFilePath))
            {
                _logger.LogInformation($"File not found {fullFilePath}");
                return string.Empty;
            }

            string content = await _fileSettingsProvider.GetFileContent(fullFilePath);

            return content;
        }
    }
}
