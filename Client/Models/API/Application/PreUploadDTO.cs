namespace Bytehide.Shield.Client.Models.API.Application
{
    public class PreUploadDto
    {
        public string ConfirmationId { get; set; }
        public bool RequiresConfirmation { get; set; }
        public string Message { get; set; }
    }
}
