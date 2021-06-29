using Shield.Client.Models;

namespace Shield.Client.Helpers
{
    internal static class ModelsMapper
    {
        public static string ToQueryString(this HubConnectionExternalModel hubConnection)
        {
            var queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);

            queryString.Add("prepare", hubConnection.TaskId);
            queryString.Add("method", hubConnection.OnLogger);

            return $"&{queryString}";
        }

        public static string ToQueryString(this ServerSentEventConnectionExternalModel sseConnection)
        {
            var queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);

            queryString.Add("id", sseConnection.TaskId);
            queryString.Add("logId", sseConnection.OnLogger);

            return $"?{queryString}";
        }

        public static string ToQueryString(this HubConnectionModel hubConnection)
        {
            var queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);

            queryString.Add("prepare", hubConnection.PrepareKey);
            queryString.Add("method", hubConnection.OnLogger);

            return $"&{queryString}";
        }
    }
}
