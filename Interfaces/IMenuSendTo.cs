using System.Drawing;

namespace Interfaces
{
    public interface IMenuSendTo
    {
        bool Enable { get; set; }
        string Name { get; set; }
        string Path { get; set; }
        string CommandLine { get; set; }
        int StatusCode { get; set; }
        string FileName { get; set; }
        bool UsedInMainWindow { get; set; }
        bool[] UsedInExplorer { get; set; }
        bool EventOnFinish { get; set; }
        Image GetImage();
    }
}
