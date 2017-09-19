using System;
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
        public async Task EnsureErrorIsLoggedIfConfigFileIsNotProvided()
        {
            //Arrange
            _fileSettingsProvider.Setup(x => x.GetFullFilePath(It.IsAny<string>(), It.IsAny<string>(), true)).Returns("hello.json");
            _fileSettingsProvider.Setup(x => x.FileExists(It.IsAny<string>())).Returns(false);
            //_logger.Setup(x => x.LogError(It.IsAny<string>(), null));
            _logger.Verify(x => x.Log(LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<FormattedLogValues>(v => v.ToString().StartsWith("File not found")),
                It.IsAny<Exception>(), It.IsAny<Func<object, Exception, string>>()));

            //Act
            var result = await _apiMockerConfigRepository.GetConfig("hello.json");

            // Assert
            //_logger.Verify(x => x.LogError(It.Is<string>(y => y.StartsWith("File not found")), null), Times.Once());
        }

    }
}
