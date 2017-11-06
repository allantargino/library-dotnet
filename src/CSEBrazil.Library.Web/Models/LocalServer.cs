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
        private List<int> Ports { get; }
        private string JsonFileSettings { get; }
        private Dictionary<string, string> ExtraSettings { get; }

        public LocalServer()
        {
            var rnd = new Random();
            Ports = new List<int>();
            for (int i = 0; i < 3; i++)
                Ports.Add(rnd.Next(1024, 65536));

            JsonFileSettings = "appsettings.json";
            ExtraSettings = new Dictionary<string, string>
            {
                { "Services:Assistant:Endpoint", $"http://localhost:{Ports[0]}"},
                { "Services:Registry:Endpoint", $"http://localhost:{Ports[1]}"},
                { "Services:Auth:Endpoint", $"http://localhost:{Ports[2]}"}
            };
        }

        public void Start()
        {
            GetWebHost<Assistant.Startup>(Ports[0]).Start();
            GetWebHost<Registry.Startup>(Ports[1]).Start();
            GetWebHost<Auth.Startup>(Ports[2]).Start();
        }

        public HttpClient GetAssistantClient() => GetClient(Ports[0]);
        public HttpClient GetRegistryClient() => GetClient(Ports[1]);
        public HttpClient GetAuthClient() => GetClient(Ports[2]);

        private HttpClient GetClient(int port)
        {
            return new HttpClient
            {
                BaseAddress = new Uri($"http://localhost:{port}/api/")
            };
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
