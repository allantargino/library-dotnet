using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace CSEBrazil.Library.Utils.Web.Interfaces
{
    public interface IServer
    {
        void Start();
        HttpClient GetAssistantClient();
        HttpClient GetAuthClient();
        HttpClient GetRegistryClient();
    }
}
