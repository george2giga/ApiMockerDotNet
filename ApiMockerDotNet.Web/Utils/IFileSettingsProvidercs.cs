using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiMockerDotNet.Web.Utils
{
    public interface IFileSettingsProvider
    {
        string GetFullFilePath(string fileName, string folder, bool isRelative = true);
        Task<string> GetFileContent(string fullFilePath);
        bool FileExists(string fullFilePath);
    }
}
