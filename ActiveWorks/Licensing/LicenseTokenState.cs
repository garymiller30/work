namespace ActiveWorks.Licensing
{
    internal sealed class LicenseTokenState
    {
        public LicenseTokenState(bool isValid, LicenseTokenPayload payload, string token, string message)
        {
            IsValid = isValid;
            Payload = payload;
            Token = token;
            Message = message;
        }

        public bool IsValid { get; }

        public LicenseTokenPayload Payload { get; }

        public string Token { get; }

        public string Message { get; }

        public bool AllowsUpdates => IsValid && Payload?.Features?.Updates == true;
    }
}
