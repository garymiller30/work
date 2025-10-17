using System;
using System.Drawing;
using System.Xml.Serialization;
using Interfaces;

namespace JobSpace.Menus
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
        
        public bool EventOnFinish { get; set; }
        [Obsolete]
        public int ChangeStatus { get; set; }
        public int StatusCode { get; set; }
        public string FileName { get; set; } = "{0}";

        public bool UsedInMainWindow { get; set; }

        public bool[] UsedInExplorer { get; set; } = new bool[4];


        public Image GetImage() => Image;
    }
}