using CSEBrazil.Library.Web.Interfaces;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace CSEBrazil.Library.Web.Models
{
    public class LocalServer : IServer
    {
        private Random random;
        private List<int> Ports { get; }
        private string JsonFileSettings { get; }
        private Dictionary<string, string> ExtraSettings { get; }
        public Dictionary<string, IWebHost> Servers { get; }

        public LocalServer()
        {
            JsonFileSettings = "appsettings.json";
            ExtraSettings = new Dictionary<string, string>();
            Servers = new Dictionary<string, IWebHost>();
            Ports = new List<int>();
            random = new Random();
        }

        public void AddService<TStartup>(string serviceName) where TStartup : class
        {
            var port = GetAvailablePort();
            Ports.Add(port);

            var serviceHost = GetWebHost<TStartup>(port);
            Servers.Add(serviceName, serviceHost);

            ExtraSettings.Add($"Services:{serviceName}:Endpoint", $"http://localhost:{port}");
        }

        public void Start()
        {
            foreach (var entry in Servers)
                entry.Value.Start();
        }

        public HttpClient GetClient(string service)
        {
            string[] keys = new string[Servers.Keys.Count];
            Servers.Keys.CopyTo(keys, 0);
            var index = Array.IndexOf(keys, service);

            return GetClient(Ports[index]);
        }

        private HttpClient GetClient(int port)
        {
            return new HttpClient
            {
                BaseAddress = new Uri($"http://localhost:{port}/api/")
            };
        }

        private int GetAvailablePort()
        {
            var port = random.Next(1024, 65536);

            return port;
        }

        private IWebHost GetWebHost<TStartup>(int port) where TStartup : class
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile(JsonFileSettings)
                .AddInMemoryCollection(ExtraSettings)
                .Build();
            
            return WebHost.CreateDefaultBuilder()
                    .UseStartup<TStartup>()
                    .UseUrls($"http://0.0.0.0:{port}")
                    .UseConfiguration(configuration)
                    .Build();
        }
    }
}
