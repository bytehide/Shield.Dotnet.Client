using System;
using System.Net;
using RestSharp;
using RestSharp.Authenticators;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Shield.Client.Models;
using JsonSerializer = System.Text.Json.JsonSerializer;
using System.Threading.Tasks;

namespace Shield.Client
{
    public class ShieldClient
    {
        private readonly string _defaultHost = "https://api.bytehide.com/api";

        public ShieldProject Project { get; set; }
        public ShieldApplication Application { get; set; }
        public ShieldTasks Tasks { get; set; }
        public ShieldConnector Connector { get; set; }
        public ShieldConfiguration Configuration { get; set; }
        public ShieldProtections Protections { get; set; }

        private RestClient Client { get; }

        internal ILogger CustomLogger { get; set; }

        internal IConfiguration ClientConfiguration { get; set; }

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
            ClientConfiguration = new ConfigurationBuilder()
                .AddJsonFile("AppSettings.json", true, true)
                .Build();

            CustomLogger = customLogger;

            //Bytehide Client
            Client = new RestClient(ClientConfiguration["url"] ?? _defaultHost)
            {
                Authenticator = new JwtAuthenticator(apiToken), Timeout = 1000 * 60 * 10
            };

            var authRequest = new RestRequest("auth/service/shield");

            var isShieldAuth = Client.Execute<ShieldAuthModel>(authRequest, Method.GET);
            
            if (isShieldAuth.Data is null || isShieldAuth.StatusCode is HttpStatusCode.Unauthorized or 0 or HttpStatusCode.Forbidden || !isShieldAuth.IsSuccessful) {
                customLogger?.LogCritical("The api token provided is invalid, the client cannot be started.");
                throw new Exception("The authorization is not correct, check the api token used or log in to your account to generate a new one.");
            }

            //Bytehide Shield Secure Client
            Client = new RestClient(isShieldAuth.Data.Host)
            {
                Authenticator = new JwtAuthenticator(isShieldAuth.Data.Token), Timeout = 1000 * 60 * 10
            };

            Client.AddDefaultHeader("x-version", ClientConfiguration["version"] ?? apiVersion);

            //var checkToken = new RestRequest("authorization/check");

            //var result = Client.Execute(checkToken, Method.GET);

            //if (result.StatusCode is HttpStatusCode.Unauthorized or 0)
            //{
            //    customLogger?.LogCritical("The api token provided is invalid, the client cannot be started.");
            //    throw new Exception("The authorization is not correct, check the api token used or log in to your account to generate a new one.");
            //}

            Project = ShieldProject.CreateInstance(Client, this);
            Application = ShieldApplication.CreateInstance(Client, this);
            Tasks = ShieldTasks.CreateInstance(Client, this);
            Connector = ShieldConnector.CreateInstance(Client, this, isShieldAuth.Data.Token, ClientConfiguration["version"] ?? apiVersion);
            Configuration = ShieldConfiguration.CreateInstance();
            Protections = ShieldProtections.CreateInstance(Client,this);

            customLogger?.LogDebug("Shield Client instantiated");
        }
        /// <summary>
        /// Check if current client has connection to the API.
        /// </summary>
        /// <param name="code">Returned connection status code.</param>
        /// <returns></returns>
        public bool CheckConnection(out HttpStatusCode code)
        {
            var checkToken = new RestRequest("api/authorization/check");

            var result = Client.Execute(checkToken, Method.GET);

            code = result.StatusCode;

            return code != HttpStatusCode.Unauthorized && code != 0;
        }

        public class AuthUserDto
        {
            public string Email { get; set; }
            public string Edition { get; set; }
        }

        public AuthUserDto GetSession()
        {
            var checkToken = new RestRequest("api/authorization/session");

            var result =  Client.Execute<AuthUserDto>(checkToken, Method.GET);

            return result.Data;
        }
    }
}
