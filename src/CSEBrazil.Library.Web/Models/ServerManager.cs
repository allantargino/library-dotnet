using CSEBrazil.Library.Web.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace CSEBrazil.Library.Web.Models
{
    public class ServerManager
    {
        private static IServer _servers;

        public static HttpClient GetAssistantClient()
        {
            CheckSingleton();
            return _servers.GetAssistantClient();
        }

        public static HttpClient GetAuthClient()
        {
            CheckSingleton();
            return _servers.GetAuthClient();
        }

        public static HttpClient GetRegistryClient()
        {
            CheckSingleton();
            return _servers.GetRegistryClient();
        }

        private static void CheckSingleton()
        {
            if (_servers == null)
            {
                _servers = new RemoteServer();
                _servers.Start();
            }
        }
    }
}
