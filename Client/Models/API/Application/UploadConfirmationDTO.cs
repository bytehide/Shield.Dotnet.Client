using System.Collections.Generic;

namespace Bytehide.Shield.Client.Models.API.Application
{
    public class UploadConfirmationDto
    {
        public bool RequiresDependencies { get; set; }
        public string Message { get; set; }

        public List<string> RequiredDependencies { get; set; }

    }
}
