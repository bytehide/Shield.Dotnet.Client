using System.Collections.Generic;
using Newtonsoft.Json;

namespace Shield.Client.Models.API.Application
{
    public class UploadConfirmationDto
    {
        public bool RequiresDependencies { get; set; }
        public string Message { get; set; }
        [JsonProperty]
        public List<string> RequiredDependencies { get; set; }

    }
}
