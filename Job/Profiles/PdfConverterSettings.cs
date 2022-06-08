using Interfaces;

namespace Job.Profiles
{
    public class PdfConverterSettings : IPdfConverterSettings
    {
        public bool MoveOriginalsToTrash { get; set; }
    }
}