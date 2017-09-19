using System;
using System.IO;
using ApiMockerDotNet.Utils;
using Xunit;

namespace ApiMockerDotNet.IntegrationTests.Utils
{
    public class FileSettingsProviderTests
    {
        private readonly FileSettingsProvider _fileSettingsProvider;
        public FileSettingsProviderTests()
        {
            _fileSettingsProvider = new FileSettingsProvider();
        }

        [Fact]
        public void EnsureRelativeFilePathIsBuilt()
        {
            //Arrange
            var folder = "app-mocks";
            var fileName = "sampleResponse1.json";
            var projectDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var expected = Path.Combine(projectDirectory,"wwwroot", folder, fileName);

            //Act
            var result = _fileSettingsProvider.GetFullFilePath(fileName, folder, true);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void EnsureFullFilePathIsBuilt()
        {
            //Arrange
            var fileName = "sampleResponse1.json";
            var projectDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var expected = Path.Combine(projectDirectory, fileName);

            //Act
            var result = _fileSettingsProvider.GetFullFilePath(fileName, projectDirectory, false);

            Assert.Equal(expected, result);
        }
    }
}
