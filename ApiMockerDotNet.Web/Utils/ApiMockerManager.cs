using ApiMockerDotNet.Web.Entities;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ApiMockerDotNet.Web.Utils
{
    public class ApiMockerManager : IApiMockerManager
    {
        private const string ConfigsFolder = "app-configs";
        private string MocksFolder = "app-mocks";
            
        private readonly ILogger logger;

        private ApiMockerConfig apiMockerConfig;

        public ApiMockerManager(ILogger logger, string configFile)
        {
            this.logger = logger;
            this.apiMockerConfig = LoadApiMockerConfig(configFile);
        }

        public ApiMockerConfig GetApiMockerConfig(string configFile)
        {
            if(apiMockerConfig == null)
            {
                this.apiMockerConfig = this.LoadApiMockerConfig(configFile);
            }

            return this.apiMockerConfig;
        }

        private ApiMockerConfig LoadApiMockerConfig(string configFile)
        {
            if (!FileExists(ConfigsFolder, configFile))
            {
                this.logger.LogError("ApiMocker cannot start without a valid config file");
                Environment.Exit(0);
            }

            var configFileFullPath = GetFullFilePath(ConfigsFolder, configFile);

            var fileContent = File.ReadAllText(configFileFullPath);
            var serializerSettings = new JsonSerializerSettings();
            serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            ApiMockerConfig apiMockerConfig = new ApiMockerConfig();

            try
            {
                var deserializedConfig = JsonConvert.DeserializeObject<ApiMockerConfig>(fileContent, serializerSettings);
                apiMockerConfig = deserializedConfig;

                //check if the mocks folder exists, if not use the default one
                if (!string.IsNullOrEmpty(apiMockerConfig.MocksFolder) && Directory.Exists(apiMockerConfig.MocksFolder))
                {
                    MocksFolder = apiMockerConfig.MocksFolder;
                }
            }
            catch
            {
                this.logger.LogInformation($"Cannot parse file {configFile}");
                Environment.Exit(0);
            }

            return apiMockerConfig;
        }

        public string GetMockContent(string fileName)
        {
            if (!FileExists(this.MocksFolder, fileName))
            {
                return "Content unavailable";
            }

            var mockFile = GetFullFilePath(this.MocksFolder, fileName);
            return File.ReadAllText(mockFile);
        }

        private static string GetFullFilePath(string folder, string fileName)
        {
            var fullFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folder, fileName);

            return fullFilePath;
        }

        private bool FileExists(string folder, string fileName)
        {
            var fullFilePath = GetFullFilePath(folder, fileName);
            if (!File.Exists(fullFilePath))
            {
                this.logger.LogWarning($"Cannot find file {fullFilePath}");
                return false;
            }

            return true;
        }
    }
}
