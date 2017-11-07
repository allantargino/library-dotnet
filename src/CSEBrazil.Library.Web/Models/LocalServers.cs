using CSEBrazil.Library.Web.Utils;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;


namespace CSEBrazil.Library.Web.Models
{
    public class LocalServers
    {
        private string JsonFileSettings { get; }

        private List<int> Ports { get; }
        private Dictionary<string, string> ExtraSettings { get; }
        private Dictionary<string, IWebHostBuilder> Servers { get; }

        private PortManager PortManager { get; }

        public LocalServers(string jsonFileSettings = "appsettings.json")
        {
            if (string.IsNullOrEmpty(nameof(jsonFileSettings))) throw new ArgumentNullException(nameof(jsonFileSettings));

            JsonFileSettings = jsonFileSettings;

            ExtraSettings = new Dictionary<string, string>();
            Servers = new Dictionary<string, IWebHostBuilder>();
            Ports = new List<int>();

            PortManager = new PortManager();
        }

        public void AddServer<TStartup>(string serviceName) where TStartup : class
        {
            if (string.IsNullOrEmpty(nameof(serviceName))) throw new ArgumentNullException(nameof(serviceName));

            var port = PortManager.GetAvailablePort();
            Ports.Add(port);

            var serviceHost = GetWebHost<TStartup>(port);
            Servers.Add(serviceName, serviceHost);

            ExtraSettings.Add($"Services:{serviceName}:Endpoint", $"http://localhost:{port}");
        }

        public void Start()
        {
            IConfigurationRoot configuration = GetConfiguration();

            foreach (var entry in Servers)
                entry.Value
                    .UseConfiguration(configuration)
                    .Build()
                    .Start();
        }

        public HttpClient GetClient(string serviceName, string prefix = "api/")
        {
            if (string.IsNullOrEmpty(nameof(serviceName))) throw new ArgumentNullException(nameof(serviceName));

            string[] keys = new string[Servers.Keys.Count];
            Servers.Keys.CopyTo(keys, 0);
            var index = Array.IndexOf(keys, serviceName);

            return GetClient(Ports[index], prefix);
        }

        private HttpClient GetClient(int port, string prefix)
        {
            return new HttpClient
            {
                BaseAddress = new Uri($"http://localhost:{port}/{prefix}")
            };
        }

        private IConfigurationRoot GetConfiguration()
        {
            return new ConfigurationBuilder()
                        .AddJsonFile(JsonFileSettings)
                        .AddInMemoryCollection(ExtraSettings)
                        .Build();
        }

        private IWebHostBuilder GetWebHost<TStartup>(int port) where TStartup : class
        {
            return WebHost.CreateDefaultBuilder()
                    .UseStartup<TStartup>()
                    .UseUrls($"http://0.0.0.0:{port}");
        }
    }
}
