namespace FtpClient
{
    public sealed class UploadFileParam
    {
        public string Password { get; set; }
        public string User { get; set; }
        public string Server { get; set; }
        public bool ActiveMode { get; set; }
        public int CodePage { get; set; }

        public string UploadFile { get; set; }
        public string TargetFtpFile { get; set; }
    }
}
