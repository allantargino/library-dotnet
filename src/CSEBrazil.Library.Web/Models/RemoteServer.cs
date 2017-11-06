using CSEBrazil.Library.Web.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace CSEBrazil.Library.Web.Models
{
    class RemoteServer : IServer
    {
        public HttpClient GetAssistantClient()
        {
            return new HttpClient
            {
                BaseAddress = new Uri("http://mv-assistant.azurewebsites.net/api/")
            };
        }

        public HttpClient GetAuthClient()
        {
            return new HttpClient
            {
                BaseAddress = new Uri("http://mv-auth.azurewebsites.net/api/")
            };
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
