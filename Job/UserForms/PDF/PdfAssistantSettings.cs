namespace JobSpace.UserForms.PDF
{
    public class PdfAssistantSettings
    {
        public string ApiUrl { get; set; } = "https://openrouter.ai/api/v1/chat/completions";
        public string SttUrl { get; set; } = "";
        public string ApiKey { get; set; } = "lm-studio";
        public string ModelName { get; set; } = "openrouter/auto";
        public string AudioModel { get; set; } = "ggml-base.bin";
    }
}
