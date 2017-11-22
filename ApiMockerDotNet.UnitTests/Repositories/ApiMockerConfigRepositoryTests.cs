using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ApiMockerDotNet.Entities;
using ApiMockerDotNet.Repositories;
using ApiMockerDotNet.Utils;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Xunit;

namespace ApiMockerDotNet.UnitTests.Repositories
{
    public class ApiMockerConfigRepositoryTests
    {
        private readonly Mock<IFileSettingsProvider> _fileSettingsProvider;
        private readonly Mock<ILogger<ApiMockerConfigRepository>> _logger;
        private readonly ApiMockerConfigRepository _apiMockerConfigRepository;

        public ApiMockerConfigRepositoryTests()
        {
            _fileSettingsProvider = new Mock<IFileSettingsProvider>();
            _logger = new Mock<ILogger<ApiMockerConfigRepository>>();
            _apiMockerConfigRepository = new ApiMockerConfigRepository(_fileSettingsProvider.Object, _logger.Object);
        }

        [Theory]
        [InlineData("sample-config.json")]
        [InlineData("sample-config.json")]
        [InlineData("sample-config.json")]
        public async Task EnsureFileNotFoundErrorIsLoggedIfConfigFileIsNotProvided(string fileName)
        {
            //Arrange
            _fileSettingsProvider.Setup(x => x.GetFullFilePath(It.IsAny<string>(), It.IsAny<string>(), true)).Returns(fileName);
            _fileSettingsProvider.Setup(x => x.FileExists(It.IsAny<string>())).Returns(false);

            //Act
            var result = await _apiMockerConfigRepository.GetConfig(fileName);

            // Assert
            _logger.VerifyLog(LogLevel.Error, s => s.StartsWith("File not found"));
        }

        [Theory]
        [InlineData("bad-config.json")]
        public async Task EnsureInvalidJSONErrorIsLoggedIfInvalidConfigFileProvided(string fileName)
        {
            //Arrange
            _fileSettingsProvider.Setup(x => x.GetFullFilePath(It.IsAny<string>(), It.IsAny<string>(), true)).Returns(fileName);
            _fileSettingsProvider.Setup(x => x.FileExists(It.IsAny<string>())).Returns(true);
            _fileSettingsProvider.Setup(x=> x.GetFileContent(fileName)).ReturnsAsync("{ \"abc : 123 }");

            //Act
            var result = await _apiMockerConfigRepository.GetConfig(fileName);

            // Assert
            _logger.VerifyLog(LogLevel.Error, s => s.StartsWith("Invalid JSON in config file"));
        }

        [Theory]
        [InlineData("valid-config.json")]
        public async Task EnsureValidConfigIsLoggedIfInvalidConfigFileProvided(string fileName)
        {
            //Arrange
            _fileSettingsProvider.Setup(x => x.GetFullFilePath(It.IsAny<string>(), It.IsAny<string>(), true)).Returns(fileName);
            _fileSettingsProvider.Setup(x => x.FileExists(It.IsAny<string>())).Returns(true);
            var dummyConfig = this.GetApiMockerConfig();
            var jsonSerializedConfig = JsonConvert.SerializeObject(dummyConfig, new JsonSerializerSettings() {ContractResolver = new CamelCasePropertyNamesContractResolver()});

         
            _fileSettingsProvider.Setup(x => x.GetFileContent(fileName)).ReturnsAsync(jsonSerializedConfig);

            //Act
            var result = await _apiMockerConfigRepository.GetConfig(fileName);

            // Assert
            result.Note.Should().BeEquivalentTo(dummyConfig.Note);
            result.ServiceMocks.ShouldBeEquivalentTo(dummyConfig.ServiceMocks);
        }

        private ApiMockerConfig GetApiMockerConfig()
        {
            return new ApiMockerConfig()
            {
                Note = "Test config",
                ServiceMocks = new List<WebServiceMock>()
                {
                    new WebServiceMock()
                    {
                        ContentType = "application/json",
                        HttpStatus = 200,
                        MockFile = "response1.json",
                        Url = "/GetFlights/Caraibbean",
                        Verb = "Get"
                    },
                    new WebServiceMock()
                    {
                        ContentType = "application/json",
                        HttpStatus = 200,
                        MockFile = "response1.json",
                        Url = "/GetBars/Havana",
                        Verb = "Get"
                    }
                }
            };
        }
    }
}
