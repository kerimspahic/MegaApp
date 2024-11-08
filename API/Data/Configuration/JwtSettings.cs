namespace API.Data.Configuration
{
    public class JwtSettings
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string SecretKey { get; set; }
        public int ExpiresInMinutes { get; set; }
        public int ExpiresInDays { get; set; }
    }
}