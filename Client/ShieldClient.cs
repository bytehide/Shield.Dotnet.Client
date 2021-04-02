using System;
using System.Net;
using RestSharp;
using RestSharp.Authenticators;
using Microsoft.Extensions.Logging;

namespace Shield.Client
{
    public class ShieldClient
    {
        //TODO: Change on production
        private const string ApiEndpoint = "https://localhost:44395/api";

        public ShieldProject Project { get; set; }
        public ShieldApplication Application { get; set; }
        public ShieldTasks Tasks { get; set; }
        public ShieldConnector Connector { get; set; }
        public ShieldConfiguration Configuration { get; set; }

        internal ILogger CustomLogger { get; set; }
        public static ShieldClient CreateInstance(string apiToken)
        {
            return new ShieldClient(apiToken, null);
        }
        public static ShieldClient CreateInstance(string apiToken, string apiVersion)
        {
            return new ShieldClient(apiToken, null, apiVersion);
        }
        public static ShieldClient CreateInstance(string apiToken, ILogger customLogger)
        {
            return new ShieldClient(apiToken, customLogger);
        }
        public static ShieldClient CreateInstance(string apiToken, ILogger customLogger, string apiVersion)
        {
            return new ShieldClient(apiToken, customLogger, apiVersion);
        }
        public ShieldClient(string apiToken, ILogger customLogger, string apiVersion = "1.1")
        {
            CustomLogger = customLogger;

            var client = new RestClient(ApiEndpoint) {Authenticator = new JwtAuthenticator(apiToken)};

            client.AddDefaultHeader("x-version", apiVersion);

            var checkToken = new RestRequest("authorization/check");

            var result = client.Execute(checkToken, Method.GET);

            if (result.StatusCode == HttpStatusCode.Unauthorized)
            {
                customLogger?.LogCritical("The api token provided is invalid, the client cannot be started.");
                throw new Exception("The authorization is not correct, check the api token used or log in to your account to generate a new one.");
            }

            Project = ShieldProject.CreateInstance(client,this);
            Application = ShieldApplication.CreateInstance(client,this);
            Tasks = ShieldTasks.CreateInstance(client,this);
            Connector = ShieldConnector.CreateInstance(client,this);
            Configuration = ShieldConfiguration.CreateInstance();

            customLogger?.LogDebug("Shield Client instantiated");
        }
    }
}
