using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RestSharp;
using Shield.Client.Fr.Extensions;
using Shield.Client.Fr.Helpers;
using Shield.Client.Fr.Models;
using Shield.Client.Fr.Models.API.Application;

namespace Shield.Client.Fr
{
    public class ShieldConnector
    {
        private readonly RestClient _client;
        public ShieldClient Parent { get; set; }

        public ShieldConnector(IRestClient client, ShieldClient parent)
        {
            _client = new RestClient(client.BaseUrl ?? throw new InvalidOperationException()) {Authenticator = client.Authenticator};
            //Not required version for logger (Only in dev).
            if (!client.BaseUrl.ToString().ToLower().StartsWith("https://api.dotnetsafer.com"))
                _client.BaseUrl = new Uri(_client.BaseUrl?.AbsoluteUri.Replace(_client.BaseUrl.PathAndQuery,null) ?? throw new InvalidOperationException());
            Parent = parent;
        }
        public static ShieldConnector CreateInstance(RestClient client)
        {
            return new ShieldConnector(client, null);
        }
        public static ShieldConnector CreateInstance(RestClient client, ShieldClient parent)
        {
            return new ShieldConnector(client, parent);
        }
        public HubConnectionExternalModel CreateHubConnection()
        {
            return CreateHubConnection(Guid.NewGuid().ToString());
        }
        public HubConnectionExternalModel CreateHubConnection(string taskId)
        {
            return new HubConnectionExternalModel { TaskId = taskId, OnLogger = Guid.NewGuid().ToString()};
        }
    }
}
