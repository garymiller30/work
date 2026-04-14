using Interfaces;

namespace JobSpace.Profiles
{
    public sealed class PdfConverterSettings : IPdfConverterSettings
    {
        public bool MoveOriginalsToTrash { get; set; }
    }
}