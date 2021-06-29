using System;
using System.Collections.Generic;
using System.Text;

namespace Shield.Client.Models
{
    public class ServerSentEventsLogMessageModel
    {
        public string Method { get; set; }
        public DateTime Time { get; set; }
        public string LogLevelString { get; set; }
        public string Message { get; set; }
    }
}
