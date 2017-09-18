using System.IO;
using System.Threading.Tasks;

namespace ApiMockerDotNet.Utils
{
    public class FileSettingsProvider : IFileSettingsProvider
    {
        public string GetFullFilePath(string fileName, string folder, bool isRelative = true)
        {
            string fullFilePath = isRelative ? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folder, fileName) : Path.Combine(folder, fileName);
            return fullFilePath;
        }

        public async Task<string> GetFileContent(string fullFilePath)
        {
            string content;
            using (var reader = File.OpenText(fullFilePath))
            {
                content = await reader.ReadToEndAsync();
            }

            return content;
        }

        public bool FileExists(string fullFilePath)
        {
            return File.Exists(fullFilePath);
        }
    }
}
