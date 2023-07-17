using System;
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
    }
}
