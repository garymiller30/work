namespace JobSpace.UserForms.PDF
{
    public class PdfAssistantSettings
    {
        public string ApiUrl { get; set; } = "http://127.0.0.1:1234/v1/chat/completions";
        public string ApiKey { get; set; } = "lm-studio";
        public string ModelName { get; set; } = "google/gemma-4-12b-qat";
    }
}
