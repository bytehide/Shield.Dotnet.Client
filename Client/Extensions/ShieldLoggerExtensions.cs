using System;
using System.Globalization;
using System.Text.Json;
using Microsoft.AspNetCore.SignalR.Client;
using Shield.Client.Models;
using Shield.Client.Models.API.Application;

namespace Shield.Client.Extensions
{
    public static class ShieldLoggerExtensions
    {
        public static void OnSuccess(this ProtectionResult result, HubConnection connection, Action<ProtectedApplicationDto> action)
            => connection.On(result.OnSuccess, action);
        public static void OnError(this ProtectionResult result, HubConnection connection, Action<string> action)
            => connection.On(result.OnError, action);
        public static void OnClose(this ProtectionResult result, HubConnection connection, Action<string> action)
            => connection.On(result.OnClose, action);
        public static void OnLog(this HubConnection connection, string id, Action<string, string, string> action)
            => connection.On(id, action);

        public static void OnSuccess(this ProtectionResult result, StartedConnection connection, Action<ProtectedApplicationDto> action)
            => connection.On(result.OnSuccess, action);
        public static void OnError(this ProtectionResult result, StartedConnection connection, Action<string> action)
            => connection.On(result.OnError, action);
        public static void OnClose(this ProtectionResult result, StartedConnection connection, Action<string> action)
            => connection.On(result.OnClose, action);
        public static void OnLog(this StartedConnection connection, string id, Action<string, string, string> action)
            => connection.On(id, action);

        public static void OnSuccess(this ProtectionResult result, QueueConnection connection, Action<ProtectedApplicationDto> action)
            => connection.On(result.OnSuccess, (message, level, time) => action(JsonSerializer.Deserialize<ProtectedApplicationDto>(message)));
        public static void OnError(this ProtectionResult result, QueueConnection connection, Action<string> action)
            => connection.On(result.OnError, (message, level, time) => action(message));
        public static void OnClose(this ProtectionResult result, QueueConnection connection, Action<string> action)
            => connection.On(result.OnClose, (message, level, time) => action(message));

        public static void OnLog(this QueueConnection connection, string id, Action<string, string, string> action)
            => connection.On(id, (message, level, time) => action(time.ToString(CultureInfo.InvariantCulture), message, level));


        public static void OnSuccess(this ServerSentEvents connection, Action<ProtectedApplicationDto> action)
            => connection.On("onSuccess", (message, level, time) => action(JsonSerializer.Deserialize<ProtectedApplicationDto>(message)));
        public static void OnError(this ServerSentEvents connection, Action<string> action)
            => connection.On("onError", (message, level, time) => action(message));
        public static void OnClose(this ServerSentEvents connection, Action<string> action)
            => connection.On("onClose", (message, level, time) => action(message));

        public static void OnLog(this ServerSentEvents connection, string id, Action<string, string, string> action)
            => connection.On(id, (message, level, time) => action(time.ToString(CultureInfo.InvariantCulture), message, level));
    }
}
