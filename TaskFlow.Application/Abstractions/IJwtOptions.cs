namespace TaskFlow.Application.Abstractions
{
    public interface IJwtOptions
    {
        public int ExpirationInMinutes { get; set; }
    }
}
