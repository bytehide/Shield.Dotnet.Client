using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.Json;
using Shield.Client.Models.API.Application;

namespace Shield.Client.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class SseLoggerExtensions
    {
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
