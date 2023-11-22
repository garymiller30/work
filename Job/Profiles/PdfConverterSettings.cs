using Interfaces;

namespace Job.Profiles
{
    public sealed class PdfConverterSettings : IPdfConverterSettings
    {
        public bool MoveOriginalsToTrash { get; set; }
    }
}