namespace EcommerceREST.Settings
{
    public class UploadSettings
    {
        public string UploadDirectory { get; set; }
        public string AllowedExtensions { get; set; }
        public long MaxFileSizeInMb { get; set; }
    }
}
