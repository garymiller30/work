using System;
using System.Drawing;
using System.Xml.Serialization;
using Interfaces;

namespace Job.Menus
{
    [Serializable]
    public sealed class MenuSendTo : IMenuSendTo
    {
        public bool Enable { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string CommandLine { get; set; }
        
        [XmlIgnore]
        [NonSerialized]
        public Image Image;
        /// <summary>
        /// чекати завершення програми
        /// </summary>
        public bool EventOnFinish { get; set; }
        [Obsolete]
        public int ChangeStatus { get; set; }
        public int StatusCode { get; set; }
        public string FileName { get; set; } = "{0}";

        public bool UsedInMainWindow { get; set; }

        public bool[] UsedInExplorer { get; set; } = new bool[4];

        public bool IsScript()
        {
            if (Path != null)
                return System.IO.Path.GetExtension(Path).Equals(".py", StringComparison.InvariantCultureIgnoreCase);
            return false;
        }

        public Image GetImage() => Image;
    }
}