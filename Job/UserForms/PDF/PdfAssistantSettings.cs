namespace JobSpace.UserForms.PDF
{
    public class PdfAssistantSettings
    {
        public string ApiUrl { get; set; } = "https://openrouter.ai/api/v1/chat/completions";
        public string ApiKey { get; set; } = "lm-studio";
        public string ModelName { get; set; } = "openrouter/auto";
    }
}
