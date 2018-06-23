using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiMockerDotNet.Utils;

namespace ApiMockerDotNet.Entities
{
    public class ConfigRepository
    {
        private readonly IFileSettingsProvider fileSettingsProvider;

        private const string ConfigsFolder = "app-configs";
        private const string MocksDefaultFolder = "app-mocks";

        public ConfigRepository(IFileSettingsProvider fileSettingsProvider)
        {
            this.fileSettingsProvider = fileSettingsProvider;
        }


    }
}
