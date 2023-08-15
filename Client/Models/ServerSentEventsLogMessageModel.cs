namespace Bytehide.Shield.Client.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class ServerSentEventsLogMessageModel
    {
        public string Id { get; set; }
        public string Time { get; set; }
        public string LogLevelString { get; set; }
        public string Message { get; set; }
    }
}
