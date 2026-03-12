namespace TaskFlow.Application.DTOs
{
    public class UserDto
    {
        public string Id { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;
        public IList<string> Roles { get; set; } = default!;
    }
}
