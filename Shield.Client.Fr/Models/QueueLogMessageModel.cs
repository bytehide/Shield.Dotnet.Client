using System;

namespace Shield.Client.Fr.Models
{
    public class QueueLogMessageModel
    {
        public string Method { get; set; }
        public DateTime Time { get; set; }
        public string LogLevelString { get; set; }
        public string Message { get; set; }
    }
}
