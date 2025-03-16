namespace Auto1040.Core.Shared
{
    public class AwsSettings
    {
        public string AccessKey { get; set; } = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID");
        public string SecretKey { get; set; } = Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY");
        public string Region { get; set; } = Environment.GetEnvironmentVariable("AWS_REGION");
    }
}
