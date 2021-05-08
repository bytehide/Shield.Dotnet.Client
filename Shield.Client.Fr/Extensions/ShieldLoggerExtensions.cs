using System;
using System.Globalization;
using System.Text.Json;
using Shield.Client.Fr.Models;
using Shield.Client.Fr.Models.API.Application;

namespace Shield.Client.Fr.Extensions
{
    public static class ShieldLoggerExtensions
    {
        public static void OnSuccess(this ProtectionResult result, QueueConnection connection, Action<ProtectedApplicationDto> action)
            => connection.On(result.OnSuccess, (message, level, time) => action(JsonSerializer.Deserialize<ProtectedApplicationDto>(message)));
        public static void OnError(this ProtectionResult result, QueueConnection connection, Action<string> action)
            => connection.On(result.OnError, (message, level, time) => action(message));
        public static void OnClose(this ProtectionResult result, QueueConnection connection, Action<string> action)
            => connection.On(result.OnClose, (message, level, time) => action(message));

        public static void OnLog(this QueueConnection connection, string id, Action<string, string, string> action)
            => connection.On(id, (message, level, time) => action(time.ToString(CultureInfo.InvariantCulture), message, level));
    }
}
