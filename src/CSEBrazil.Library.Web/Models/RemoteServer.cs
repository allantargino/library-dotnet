using CSEBrazil.Library.Web.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace CSEBrazil.Library.Web.Models
{
    class RemoteServer : IServer
    {

        public void AddService<TStartup>(string serviceName) where TStartup : class
        {
            throw new NotImplementedException();
        }

        public HttpClient GetClient(string service)
        {
            throw new NotImplementedException();
        }

        public HttpClient GetRegistryClient()
        {
            return new HttpClient
            {
                BaseAddress = new Uri("http://mv-registry.azurewebsites.net/api/")
            };
        }

        public void Start() { }
    }
}
