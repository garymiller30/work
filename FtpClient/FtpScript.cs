using Interfaces;

namespace FtpClient
{
    public sealed class FtpScript : IFtpScript
    {
        public bool Enable { get; set; } = true;
        public string ScriptPath { get; set; }
        public string Name { get; set; }
        public string Parameters { get; set; }
    }
}
