using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ApiMockerDotNet.Web.Repositories
{
    public class ResponseContentRepository
    {
        private readonly ILogger _logger;

        public ResponseContentRepository(ILogger logger)
        {
            _logger = logger;
        }

        public async Task<string> Get(string fileName, string folder)
        {
            if (string.IsNullOrEmpty(folder))
            {
                _logger.LogWarning($"Filename is empty");
                return string.Empty;
            }

            var fullFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folder, fileName);
            if (!File.Exists(fullFilePath))
            {
                _logger.LogWarning($"File not found {fullFilePath}");
                return string.Empty;
            }
            string content;
            using (var reader = File.OpenText(fullFilePath))
            {
                content = await reader.ReadToEndAsync();
            }

            return content;
        }
    }
}
