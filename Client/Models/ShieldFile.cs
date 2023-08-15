namespace Bytehide.Shield.Client.Models
{
    /// <summary>
    /// Work-Standard model of custom file for shield.
    /// </summary>
    public class ShieldFile
    {
        public string FileName { get; set; }
        public byte[] FileContent { get; set; }
    }
}
