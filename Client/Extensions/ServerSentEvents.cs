using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Bytehide.Shield.Client.Helpers;
using Bytehide.Shield.Client.Models;
using Bytehide.Shield.Client.Models.API;
using Newtonsoft.Json;
// using System.Text.Json;
// using System.Text.Json.Serialization;

namespace Bytehide.Shield.Client.Extensions
{
    //TODO: Perform actions flow:
    /// <summary>
    /// 
    /// </summary>
    public class ServerSentEvents
    {
        private readonly string[] _actionNames = {"protect_single_app_directly"};

        private Dictionary<string, Action<string, string, DateTime>> Methods { get; } = new();

        private Dictionary<string, Action> InternalEvents { get; } = new();

        private readonly HttpClient _client = new();

        private CancellationToken _cancellationToken;

        private CancellationTokenSource _cancellationTokenSource;

        #region SSE FLOW

        /// <summary>
        /// 
        /// </summary>
        public Action BeforeConnect { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Action OnConnected { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Action BeforeDisconnect { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Action OnDisconnect { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public Action AfterDisconnect { get; set; }

        #endregion

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
        public void ProtectSingleFile(string projectKey, string fileBlob, ProtectionConfigurationDTO configuration)
        {
            BeforeConnect?.Invoke();

            var taskUrl = $"{_baseUrl}api/protection/protect/single/sse?projectKey={projectKey}&fileBlob={fileBlob}&onlLog={_defaultLoggerId}";

            if (configuration is not null)
            {
                // var jsonConfiguration = JsonSerializer.Serialize(configuration);
                var jsonConfiguration = JsonConvert.SerializeObject(configuration);
                taskUrl += $"&applicationConfigurationDto={WebUtility.UrlEncode(jsonConfiguration)}";
            }

            var request = WebRequest.Create(new Uri(taskUrl));
            request.Headers["Authorization"] = $"Bearer {_bearerToken}";
            request.Headers.Add("x-version", _apiVersion);
            ((HttpWebRequest)request).AllowReadStreamBuffering = false;
            request.Timeout = Timeout.Infinite;

            var response = request.GetResponse();

            var stream = response.GetResponseStream();

            using var reader = new StreamReader(stream ?? throw new InvalidOperationException("Can't connect with Bytehide Shield service"));

            OnDisconnect = delegate
            {
                // ReSharper disable once AccessToDisposedClosure
                reader.Close();
                AfterDisconnect?.Invoke();
                _cancellationTokenSource = new CancellationTokenSource();
                _cancellationToken = _cancellationTokenSource.Token;
            };

            try
            {
                while (!reader.EndOfStream)
                {
                    _cancellationToken.ThrowIfCancellationRequested();

                    var text = reader.ReadLine();

                    ReceiverOnProcessMessage(text, _actionNames[0]);
                }
            }
            catch (OperationCanceledException)
            {
                BeforeDisconnect?.Invoke();
                OnDisconnect?.Invoke();
            }
            catch (ObjectDisposedException)
            {
                //ignored, OnDisconnect was fired
            }
            catch (Exception ex) when (ex.Message.ToLowerInvariant().Contains("cannot read from a closed textreader"))
            {
                //ignored, OnDisconnect was fired
            }
        }
        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectKey"></param>
        /// <param name="fileBlob"></param>
        /// <param name="configuration"></param>
        public async Task ProtectSingleFileAsync(string projectKey, string fileBlob, ProtectionConfigurationDTO configuration)
        {

            BeforeConnect?.Invoke();

            var taskUrl = $"{_baseUrl}api/protection/protect/single/sse?projectKey={projectKey}&fileBlob={fileBlob}&onlLog={_defaultLoggerId}";

            if (configuration is not null)
            {
                var jsonConfiguration =  JsonConvert.SerializeObject(configuration);
                taskUrl += $"&applicationConfigurationDto={WebUtility.UrlEncode(jsonConfiguration)}";
            }

            using var streamReader = new StreamReader(await _client.GetStreamAsync(taskUrl) ?? throw new InvalidOperationException("Can't connect with Bytehide Shield service"));

            OnDisconnect = delegate
            {
                // ReSharper disable once AccessToDisposedClosure
                streamReader.Close();
                AfterDisconnect?.Invoke();
                _cancellationTokenSource = new CancellationTokenSource();
                _cancellationToken = _cancellationTokenSource.Token;
            };

            try
            {
                while (!streamReader.EndOfStream)
                {
                    _cancellationToken.ThrowIfCancellationRequested();

                    var message = await streamReader.ReadLineAsync();

                    ReceiverOnProcessMessage(message, _actionNames[0]);
                }
            }
            catch (OperationCanceledException)
            {
                BeforeDisconnect?.Invoke();
                OnDisconnect?.Invoke();
            }
            catch (ObjectDisposedException)
            {
                //ignored, OnDisconnect was fired
            }
            catch (Exception ex) when(ex.Message.ToLowerInvariant().Contains("cannot read from a closed textreader"))
            {
                //ignored, OnDisconnect was fired
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Stop()
        {
           _cancellationTokenSource.Cancel();
        }

        private void ReceiverOnProcessMessage(string msg, string eventName = null)
        {
            if (msg is not null && msg.StartsWith("data:"))
                msg = msg.Remove(0, 5);
            try
            {
                switch (msg)
                {
                    case "connected":
                        OnConnected?.Invoke();
                        return;
                    case "disconnected":
                        BeforeDisconnect?.Invoke();
                        OnDisconnect?.Invoke();
                        return;
                }

                // var message = JsonSerializer.Deserialize<ServerSentEventsLogMessageModel>(msg);
                if (msg == null) return;
                var message = JsonConvert.DeserializeObject<ServerSentEventsLogMessageModel>(msg);

                if (message is null)
                    return;

                if (eventName == "protect_single_app_directly")
                    switch (message.Id)
                    {
                        case "update":
                            UpdateEvents<ProtectResponse>(message.Message);
                            break;
                        case "error":
                            ParseInitError(message.Message);
                            break;
                    }

                if (Methods.TryGetValue(message.Id, out var action))
                    action?.Invoke(message.Message, message.LogLevelString, Convert.ToDateTime(message.Time));

                if (InternalEvents.TryGetValue(message.Id, out var eventAction))
                    eventAction?.Invoke();
            }
            catch (ShieldInitException)
            {
                throw;
            }
            catch
            {
                //ignored
            }
        }

        #region Events Reader for Protect Single App action.

        // ReSharper disable once ClassNeverInstantiated.Local
        private class ProtectResponse
        {
            public string onSuccess { get; set; }
            public string onError { get; set; }
            public string onClose { get; set; }
        }

        private void UpdateEvents<T>(string updateData) where T : ProtectResponse
        {
            // var updates = JsonSerializer.Deserialize<T>(updateData);
            var updates = JsonConvert.DeserializeObject<T>(updateData);

            if (updates == null) return;

            var success = Methods.FirstOrDefault(action => action.Key == "onSuccess");
            if (success.Value is not null)
                Methods.Add(updates.onSuccess, success.Value);
            var error = Methods.FirstOrDefault(action => action.Key == "onError");
            if (error.Value is not null)
                Methods.Add(updates.onError, error.Value);
            var close = Methods.FirstOrDefault(action => action.Key == "onClose");
            if (close.Value is not null)
                Methods.Add(updates.onClose, close.Value);

            
            InternalEvents.Add(updates.onClose, delegate { 
                BeforeDisconnect?.Invoke(); 
                OnDisconnect?.Invoke();
            });
        }

        private void ParseInitError(string error)
        {
            BeforeDisconnect?.Invoke();
            OnDisconnect?.Invoke();
            throw new ShieldInitException(error);
        }


        #endregion

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
