namespace RunAway.API.RequestViewModel
{
    public class LoginRequestViewModel
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string Name { get; set; }
    }
}
