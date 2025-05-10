namespace RunAway.Infrastructure.Authentication
{
    public class JwtSettings
    {
        public string Key { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public int DurationInHours { get; set; } = 1;

        public static JwtSettings LoadFromEnvironment()
        {
            return new JwtSettings
            {
                Key = Environment.GetEnvironmentVariable("JWT_KEY") ??
                    throw new InvalidOperationException("JWT_KEY environment variable is not set"),

                Issuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ??
                    throw new InvalidOperationException("JWT_ISSUER environment variable is not set"),

                Audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ??
                    throw new InvalidOperationException("JWT_AUDIENCE environment variable is not set"),

                DurationInHours = int.TryParse(Environment.GetEnvironmentVariable("JWT_DURATION_HOURS"), out int hours)
                    ? hours
                    : 1 // Default to 1 hour if not specified
            };
        }
    }
}
