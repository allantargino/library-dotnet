# Library.Web

## Usage

```cs
static class ServerManager
{
    private static LocalServers _servers;

    static ServerManager()
    {
        _servers = new LocalServers();

        _servers.AddServer<Assistant.Startup>("Assistant");
        _servers.AddServer<Auth.Startup>("Auth");
        _servers.AddServer<Registry.Startup>("Registry");

        _servers.Start();
    }

    public static HttpClient GetClient(string service)
    {
        return _servers.GetClient(service);
    }

}
```

```cs
public class RegistryTest
{
    HttpClient client = ServerManager.GetClient("Registry");

    [Fact]
    public async Task ExecutionFlow()
    {
        [...]
    }
}
```