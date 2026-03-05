namespace TaskFlow.Api.Contracts
{
    public class UserRegistrationRequest : UserLoginRequest
    {
        public required string Name { get; set; }
    }
}
