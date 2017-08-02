using ApiMockerDotNet.Web.Entities;

namespace ApiMockerDotNet.Web.Utils
{
    public interface IApiMockerManager
    {
        ApiMockerConfig GetApiMockerConfig(string configFile);
        string GetMockContent(string fileName);
    }
}