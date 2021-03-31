using Newtonsoft.Json;

namespace Shield.Client.Models
{
    public class NegotiateModel
    {
        [JsonRequired]
        public string AccessToken { get; set; }
        [JsonRequired]
        public string Url { get; set; }
    }
}
