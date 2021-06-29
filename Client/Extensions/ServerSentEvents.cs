using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Shield.Client.Helpers;
using Shield.Client.Models;

namespace Shield.Client.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public class ServerSentEvents
    {
        private Dictionary<string, Action<string, string, DateTime>> Methods { get; } = new();

        private readonly HttpClient _client = new();

        private CancellationToken _cancellationToken;

        private readonly CancellationTokenSource _cancellationTokenSource;

        private readonly string _connectionHost;

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
        /// <param name="method"></param>
        public void Destroy(string method) => Methods.Remove(method);

        /// <summary>
        /// 
        /// </summary>
        public async Task StartAsync()
        {
            using var streamReader = new StreamReader(await _client.GetStreamAsync(_connectionHost));

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
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Start()
        {
            using var streamReader = new StreamReader(_client.GetStreamAsync(_connectionHost).Result);

            try
            {
                while (!streamReader.EndOfStream)
                {
                    _cancellationToken.ThrowIfCancellationRequested();

                    var message = streamReader.ReadLine();

                    ReceiverOnProcessMessage(message);
                }

            }
            catch (OperationCanceledException)
            {
                streamReader.Close();
                streamReader.Dispose();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void Stop()
        {
           _cancellationTokenSource.Cancel();
        }

        private void ReceiverOnProcessMessage(string msg)
        {
            try
            {
                var message = JsonSerializer.Deserialize<ServerSentEventsLogMessageModel>(msg);

                if (message is null)
                    return;

                if (!Methods.TryGetValue(message.Method, out var action))
                    return;

                action(message.Message, message.LogLevelString, message.Time);
            }
            catch
            {
                //ignored
            }
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionData"></param>
        /// <param name="url"></param>
        public ServerSentEvents(ServerSentEventConnectionExternalModel connectionData, string url)
        {
            _client.Timeout = TimeSpan.FromSeconds(5);
            _connectionHost = $"{url}{connectionData.ToQueryString()}";

            _cancellationTokenSource = new CancellationTokenSource();
            _cancellationToken = _cancellationTokenSource.Token;
        }
    }
}
