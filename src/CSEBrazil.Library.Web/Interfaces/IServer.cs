using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace CSEBrazil.Library.Web.Interfaces
{
    public interface IServer
    {
        void Start();
        HttpClient GetClient(string service);

        void AddService<TStartup>(string serviceName) where TStartup : class;
    }
}
