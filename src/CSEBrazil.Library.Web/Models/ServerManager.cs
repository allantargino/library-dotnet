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

        public ServerManager()
        {

        }

        public static HttpClient GetClient(string service)
        {
            CheckSingleton();
            return _servers.GetClient(service);
        }

        private static void CheckSingleton()
        {
            if (_servers == null)
            {
                _servers = new LocalServer();
                
                _servers.Start();
            }
        }
    }
}
