using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace CSEBrazil.Library.Web.Models
{
    public class RemoteServers
    {
        public Dictionary<string, string> Services { get; set; }

        public RemoteServers()
        {
            Services = new Dictionary<string, string>();
        }

        public void AddServer(string serviceName, string serviceUrl)
        {
            if (string.IsNullOrEmpty(nameof(serviceName))) throw new ArgumentNullException(nameof(serviceName));
            if (string.IsNullOrEmpty(nameof(serviceUrl))) throw new ArgumentNullException(nameof(serviceUrl));


            Services.Add(serviceName, serviceUrl);
        }

        public HttpClient GetClient(string serviceName, string prefix)
        {
            if (string.IsNullOrEmpty(nameof(serviceName))) throw new ArgumentNullException(nameof(serviceName));
            if (string.IsNullOrEmpty(nameof(prefix))) throw new ArgumentNullException(nameof(prefix));

            return new HttpClient
            {
                BaseAddress = new Uri($"{Services[serviceName]}/{prefix}")
            };
        }
    }
}
