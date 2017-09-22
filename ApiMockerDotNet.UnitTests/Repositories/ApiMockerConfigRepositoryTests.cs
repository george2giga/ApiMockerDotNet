using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ApiMockerDotNet.Repositories;
using ApiMockerDotNet.Utils;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Internal;
using Moq;
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

        [Fact]
        public async Task EnsureFileNotFoundErrorIsLoggedIfConfigFileIsNotProvided()
        {
            //Arrange
            _fileSettingsProvider.Setup(x => x.GetFullFilePath(It.IsAny<string>(), It.IsAny<string>(), true)).Returns("hello.json");
            _fileSettingsProvider.Setup(x => x.FileExists(It.IsAny<string>())).Returns(false);

            //Act
            var result = await _apiMockerConfigRepository.GetConfig("hello.json");

            // Assert
            _logger.VerifyLog(LogLevel.Error, s => s.StartsWith("File not found"));
        }

        [Fact]
        public async Task EnsureInvalid JSONErrorIsLoggedIfInvalidConfigFileProvided()
        {
            //Arrange
            var badFormatFileName = "badFormatConfig.json";
            _fileSettingsProvider.Setup(x => x.GetFullFilePath(It.IsAny<string>(), It.IsAny<string>(), true)).Returns(badFormatFileName);
            _fileSettingsProvider.Setup(x => x.FileExists(It.IsAny<string>())).Returns(false);
            _fileSettingsProvider.Setup(x=> x.GetFileContent(badFormatFileName)).ReturnsAsync("badformat.json");

            //Act
            var result = await _apiMockerConfigRepository.GetConfig("hello.json");

            // Assert
            _logger.VerifyLog(LogLevel.Error, s => s.StartsWith("File not found"));
        }
    }
}
