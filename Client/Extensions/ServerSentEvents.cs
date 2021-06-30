using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using ServiceStack;
using Shield.Client.Models;
using Shield.Client.Models.API.Application;

namespace Shield.Client.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public class ServerSentEvents
    {
        private Dictionary<string, Action<string, string, DateTime>> Methods { get; } = new();

        private Dictionary<string, Action> Events { get; } = new();


        private readonly HttpClient _client = new();

        private CancellationToken _cancellationToken;

        private readonly CancellationTokenSource _cancellationTokenSource;

        /// <summary>
        /// 
        /// </summary>
        public Action OnConnected { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Action OnClosed { get; set; }

        private readonly string _defaultLoggerId, _baseUrl, _bearerToken, _apiVersion;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="method"></param>
        /// <param name="action"></param>
        public void On(string method, Action<string, string, DateTime> action) =>
            Methods.Add(method, action);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        public void SetDefaultLogger(Action<string, string, DateTime> action) =>
            Methods.Add(_defaultLoggerId, action);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="method"></param>
        public void Destroy(string method) => Methods.Remove(method);

        /// <summary>
        /// 
        /// </summary>
        public void ProtectSingleFile(string projectKey, string fileBlob, ApplicationConfigurationDto configuration)
        {
            var taskUrl = $"{_baseUrl}protection/protect/single/sse?projectKey={projectKey}&fileBlob={fileBlob}&onlLog={_defaultLoggerId}";

            var request = WebRequest.Create(new Uri(taskUrl));
            request.AddBearerToken(_bearerToken);
            request.Headers.Add("x-version", _apiVersion);
            ((HttpWebRequest)request).AllowReadStreamBuffering = false;

            var response = request.GetResponse();
            var stream = response.GetResponseStream();
          
            
            var encoder = new UTF8Encoding();
            var buffer = new byte[2048];
            try
            {
                while (stream is { CanRead: true })
                {
                    _cancellationToken.ThrowIfCancellationRequested();

                    var len = stream.Read(buffer, 0, 2048);
                    if (len <= 0) continue;
                    var text = encoder.GetString(buffer, 0, len);
                    ReceiverOnProcessMessage(text);
                }
            }
            catch (OperationCanceledException)
            {
                stream?.Close();
                stream?.Dispose();
                OnClosed?.Invoke();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectKey"></param>
        /// <param name="fileBlob"></param>
        /// <param name="configuration"></param>
        public async Task ProtectSingleFileAsync(string projectKey, string fileBlob, ApplicationConfigurationDto configuration)
        {
            var taskUrl = $"{_baseUrl}protection/protect/single/sse?projectKey={projectKey}&fileBlob={fileBlob}&onlLog={_defaultLoggerId}";

            using var streamReader = new StreamReader(await _client.GetStreamAsync(taskUrl));

            try
            {
                while (!streamReader.EndOfStream)
                {
                    _cancellationToken.ThrowIfCancellationRequested();

                    var message = await streamReader.ReadLineAsync();

                    ReceiverOnProcessMessage(message);
                }
            }
            catch (OperationCanceledException)
            {
                streamReader.Close();
                streamReader.Dispose();
                OnClosed?.Invoke();
            }
            catch (Exception s)
            {
                var f = 0;
                //ignored
            }
        }



        /// <summary>
        /// 
        /// </summary>
        //public void Start()
        //{
        //    using var streamReader = new StreamReader(_client.GetStreamAsync(_connectionHost).Result);

        //    try
        //    {
        //        while (!streamReader.EndOfStream)
        //        {
        //            _cancellationToken.ThrowIfCancellationRequested();

        //            var message = streamReader.ReadLine();

        //            ReceiverOnProcessMessage(message);
        //        }

        //    }
        //    catch (OperationCanceledException)
        //    {
        //        streamReader.Close();
        //        streamReader.Dispose();
        //    }
        //}
        /// <summary>
        /// 
        /// </summary>
        public void Stop()
        {
           _cancellationTokenSource.Cancel();
        }

        private class ProtectResponse
        {
            public string onSuccess { get; set; }
            public string onError { get; set; }
            public string onClose { get; set; }
        }
        private void UpdateEvents(string updateData)
        {
            var updates = JsonSerializer.Deserialize<ProtectResponse>(updateData);

            if (updates == null) return;

            var success = Methods.First(action => action.Key == "onSuccess");
            Methods.Add(updates.onSuccess, success.Value);
            var error = Methods.First(action => action.Key == "onError");
            Methods.Add(updates.onError, error.Value);
            var close = Methods.First(action => action.Key == "onClose");
            Methods.Add(updates.onClose, close.Value);

            Events.Add(updates.onClose, () => _cancellationTokenSource.Cancel());

        }

        private void ReceiverOnProcessMessage(string msg)
        {
            try
            {
                if (msg == "connected") {
                    OnConnected?.Invoke();
                    return;
                }

                var message = JsonSerializer.Deserialize<ServerSentEventsLogMessageModel>(msg);

                if (message is null)
                    return;

                if (message.Id == "update")
                {
                    UpdateEvents(message.Message);
                }

                
                if (Methods.TryGetValue(message.Id, out var action))
                    action(message.Message, message.LogLevelString, Convert.ToDateTime(message.Time));

                if (Events.TryGetValue(message.Id, out var eventAction))
                    eventAction();
            }
            catch
            {
                //ignored
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="authToken"></param>
        /// <param name="version"></param>
        /// <param name="baseUrl"></param>
        public ServerSentEvents(string authToken, string version, string baseUrl)
        {
            _client.Timeout = TimeSpan.FromSeconds(5);

            _baseUrl = baseUrl;

            _defaultLoggerId = Guid.NewGuid().ToString();

            _client.DefaultRequestHeaders.Authorization
                = new AuthenticationHeaderValue("Bearer", authToken);
            _client.DefaultRequestHeaders.Add("x-version", version);

            _bearerToken = authToken;
            _apiVersion = version;

            _cancellationTokenSource = new CancellationTokenSource();
            _cancellationToken = _cancellationTokenSource.Token;
        }
    }
}
