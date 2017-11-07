using System;
using System.Net.NetworkInformation;
using System.Linq;

namespace CSEBrazil.Library.Web.Utils
{
    public class PortManager
    {
        public int MaxTentatives { get; }
        public Random Random { get; }

        public PortManager(int maxTentatives = 10)
        {
            MaxTentatives = maxTentatives;
            Random = new Random();
        }

        public int GetAvailablePort()
        {
            var tentative = 0;

            do
            {
                var selectedPort = Random.Next(1024, 65536);

                var properties = IPGlobalProperties.GetIPGlobalProperties();
                var ports = properties.GetActiveTcpConnections();

                if (ports.Where(p => p.LocalEndPoint.Port == selectedPort).Count() == 0)
                    return selectedPort;
                else
                    tentative++;

            } while (tentative < MaxTentatives);

            throw new InvalidOperationException("Could not assigned a port!");
        }
    }
}
