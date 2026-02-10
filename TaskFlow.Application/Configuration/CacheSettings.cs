namespace TaskFlow.Application.Configuration
{
    public class CacheSettings
    {
        public int UserCacheExpirationMinutes { get; set; } = 5;
        public int MembershipCacheExpirationMinutes { get; set; } = 1;
    }
}
